using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private BombSkill _bombSkill;
    private CinemachineImpulseSource _impulseSource;
    private Collider[] _colliders;
    private ParticleSystem _explosionParticle;
    [SerializeField] private Material _material;
    [SerializeField, ColorUsage(true,true)] private Color _defaultColor;
    [SerializeField, ColorUsage(true,true)] private Color _explosionColor;
    public void Initialize(BombSkill bombSkill)
    {
        _material.SetFloat("_Alpha", 1);
        _material.color = _defaultColor;
        _bombSkill = bombSkill;
        transform.rotation = Quaternion.Euler(-90, 0, 0);
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _explosionParticle = GetComponentInChildren<ParticleSystem>();
        _colliders = new Collider[10];
    }
    
    public void Explode(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
        _impulseSource.GenerateImpulse();
        StartCoroutine(DelayDamageCoroutine());
    }

    private IEnumerator DelayDamageCoroutine()
    {
        yield return new WaitForSeconds(3f);
        int count = CheckEnemy();
        for (int i = 0; i < count; i++)
        {
            if (_colliders[i].TryGetComponent(out IDamageable health))
            {
                Vector3 point = _colliders[i].transform.position;
                health.ApplyDamage(_bombSkill.damage, point, Vector3.up, 0,
                    _bombSkill.player, DamageType.Bomb);
                Debug.Log("Bomb Damage");
                StartCoroutine(ParticlePlay());
            }
        }

        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    private IEnumerator ParticlePlay()
    {
        _explosionParticle.transform.position = _colliders[0].transform.position;
        _explosionParticle.Play();
        _material.DOFloat(0, "_Alpha", 1f);
        _material.DOColor(_explosionColor, 1f);
        yield return new WaitForSeconds(1f);
        _explosionParticle.Stop();
    }

    private int CheckEnemy()
    {
        return Physics.OverlapSphereNonAlloc(transform.position, _bombSkill.hitRadius,
            _colliders, _bombSkill.whatIsEnemy);
    }
}

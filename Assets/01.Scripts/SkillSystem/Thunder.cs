using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _particles;
    private ThunderStrikeSkill _skill;

    private Collider[] _colliders;
    
    public void Initialize(ThunderStrikeSkill skill)
    {
        _skill = skill;
        gameObject.SetActive(false);
        _colliders = new Collider[skill.maxHitEnemyCount];
    }

    public void PlayEffect()
    {
        foreach (var particle in _particles)
        {
            particle.Stop();
            particle.Simulate(0);
            particle.Play();
        }
    }

    public void Strike(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
        PlayEffect();

        StartCoroutine(DelayDamageCoroutine());
    }

    private IEnumerator DelayDamageCoroutine()
    {
        float thunderDelay = 0.25f;
        yield return new WaitForSeconds(thunderDelay);
        int count = CheckEnemy();
        for (int i = 0; i < count; i++)
        {
            if (_colliders[i].TryGetComponent(out IDamageable health))
            {
                Vector3 point = _colliders[i].transform.position;
                health.ApplyDamage(_skill.damage, point, Vector3.up, 0,
                    _skill.player, DamageType.Projectile);
            }
        }

        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    private int CheckEnemy()
    {
        return Physics.OverlapSphereNonAlloc(transform.position, _skill.hitRadius,
            _colliders, _skill.whatIsEnemy);
    }
}

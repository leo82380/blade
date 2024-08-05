using DG.Tweening;
using ObjectPooling;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum SatelliteState
{
    Follow, 
    Shoot, 
    Show,
    Hide
}

public class Satellite : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;

    private SatelliteSkill _skill;

    private Transform _playerAddOnTrm;
    private Vector3 _offset;

    private float _nextFireTime;
    private SatelliteState _state;
    private Collider[] _colliders;

    private readonly int _hashDissolveHeight 
        = Shader.PropertyToID("_DissolveHeight");

    private Vector3 _lastFollowPosition;

    public void Initialize(SatelliteSkill skill, Vector3 offset)
    {
        _skill = skill;
        _playerAddOnTrm = skill.player.transform.Find("AddOnTrm");
        _offset = offset;

        _nextFireTime = 0;

        _state = SatelliteState.Hide;
        _colliders = new Collider[1]; //한번에 한개의 대상만 공격할거라서

        gameObject.SetActive(false);
    }

    #region Dissolve section
    private void DissolveProcess(float end, float duration, TweenCallback Callback)
    {
        Material mat = _meshRenderer.material;
        mat.DOFloat(end, _hashDissolveHeight, duration).OnComplete(Callback);
    }

    public void ShowProcess()
    {
        gameObject.SetActive(true);
        transform.position = _playerAddOnTrm.position + _offset;

        _lastFollowPosition = GetRandomFollowPosition();

        _state = SatelliteState.Show;
        DissolveProcess(2f, 1f, () => _state = SatelliteState.Follow);
    }

    public void HideProcess()
    {
        _state = SatelliteState.Hide;
        DissolveProcess(-2f, 1f, () => gameObject.SetActive(false));
    }
    #endregion

    private Vector3 GetRandomFollowPosition()
    {
        return _playerAddOnTrm.position + _offset 
            + UnityEngine.Random.insideUnitSphere * 0.8f;
    }


    private void Update()
    {
        if(_state == SatelliteState.Follow 
            && Time.time >= _nextFireTime
            && CheckEnemy())
        {
            FireProcess();
        }
    }

    private void FireProcess()
    {
        _state = SatelliteState.Shoot;
        float randomOffset = 0.3f;

        _nextFireTime = Time.time + Random.Range(-randomOffset, randomOffset)
                        + _skill.fireCooldown;
        StartCoroutine(DelayShoot());
    }

    private IEnumerator DelayShoot()
    {
        Transform targetTrm = _colliders[0].transform;
        Vector3 start = transform.position;
        Vector3 target = targetTrm.position + new Vector3(0, 0.8f);
        Vector3 direction = target - start;

        if(Physics.Raycast(start, direction.normalized, out RaycastHit hit, 
            _skill.checkRadius, _skill.whatIsEnemy))
        {
            transform.rotation = Quaternion.LookRotation(direction.normalized);

            //여기서 트레일 그려주고
            LaserTrail trail = PoolManager.Instance.Pop(PoolingType.VFX_Trail) 
                                as LaserTrail;
            trail.DrawTrail(transform.position, hit.point, 0.1f);


            if(hit.collider.TryGetComponent(out IDamageable health))
            {
                health.ApplyDamage(_skill.damage, hit.point, hit.normal, _skill.knockBackPower, _skill.player, DamageType.Range);
            }
        }

        yield return new WaitForSeconds(0.15f);
        transform.rotation = Quaternion.identity;
        _state = SatelliteState.Follow;
    }

    private bool CheckEnemy()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, _skill.checkRadius, _colliders, _skill.whatIsEnemy);
        return count >= 1;
    }

    private void LateUpdate()
    {
        float distance = Vector3.Distance(_lastFollowPosition, transform.position);

        if(_state == SatelliteState.Follow 
            && distance < 0.1f)
        {
            _lastFollowPosition = GetRandomFollowPosition();
        }

        float moveSpeed = _skill.moveSpeed * Mathf.Clamp(distance / 3f, 0.3f, 1f);
        transform.position = Vector3.MoveTowards(
            transform.position, _lastFollowPosition, Time.deltaTime * moveSpeed);
    }
}

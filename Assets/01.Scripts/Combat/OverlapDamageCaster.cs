using System;
using UnityEngine;

public class OverlapDamageCaster : DamageCaster
{
    [SerializeField] protected float _castRadius;
    [SerializeField] protected int _maxColliderCount = 1;
    
    private Collider[] _colliders;

    public override void InitCaster(Agent agent)
    {
        base.InitCaster(agent);
        _colliders = new Collider[_maxColliderCount];
    }

    public override bool CastDamage()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, _castRadius, _colliders, targetLayer);

        for (int i = 0; i < count; i++)
        {
            Transform target = _colliders[i].transform;
            if (target.TryGetComponent(out IDamageable health))
            {
                int damage = _owner.Stat.GetDamage();
                float knockbackPower = 3f; // 나중에 스탯에서 뽑기
                
                Vector3 dir = (target.position + Vector3.up) - transform.position;
                if (Physics.Raycast(
                        transform.position, dir, out RaycastHit hit, dir.magnitude, targetLayer))
                {
                    health.ApplyDamage(damage, hit.point, hit.normal, knockbackPower, _owner, DamageType.Melee);
                }
                else
                {
                    health.ApplyDamage(damage, target.position, -dir.normalized, knockbackPower, _owner, DamageType.Melee);
                }
            }
        }
        
        OnCastDamageEvent?.Invoke();
        return count >= 1;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _castRadius);
        Gizmos.color = Color.white;
    }
}
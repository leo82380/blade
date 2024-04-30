using System;
using UnityEngine;

public class DamageOrb : MonoBehaviour
{
    private CircleOrbSkill _skill;

    private Transform _parentTrm;
    private float _currentCooldown;
    
    public void InitializeOrb(CircleOrbSkill skill)
    {
        _skill = skill;
        _parentTrm = transform.parent;
        _currentCooldown = 0;
    }

    private void Update()
    {
        if (_currentCooldown > 0)
        {
            _currentCooldown -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_currentCooldown > 0) return;
        Debug.Log(other.name);

        if (other.TryGetComponent(out IDamageable health))
        {
            _currentCooldown = _skill.orbCooldown;
            Vector3 normal = _parentTrm.position - transform.position;
            health.ApplyDamage(_skill.damage, transform.position, normal,
                _skill.knockbackPower, _skill.player, DamageType.Projectile);
        }
    }
}

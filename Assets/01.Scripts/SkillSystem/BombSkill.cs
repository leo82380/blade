using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BombSkill : Skill
{
    public int damage = 5;
    public int maxDamage = 10;
    public float hitRadius = 100f;
    public float detectRadius = 7f;
    
    [SerializeField] private Bomb _prefab;

    public bool CanUpgradeBombDamage()
    {
        return damage < maxDamage;
    }
    
    public void UpgradeBombDamage(int value)
    {
        if (damage >= maxDamage) return;
        
        damage += value;
    }
    
    public override bool UseSkill()
    {
        if (_cooldownTimer > 0 || skillEnabled == false) return false;
        if (base.UseSkill() == false) return false;
        
        _cooldownTimer = _cooldown;
        
        Bomb bomb = Instantiate(_prefab, transform);
        bomb.Initialize(this);
        bomb.Explode(transform.position);
        
        return true;
    }

}

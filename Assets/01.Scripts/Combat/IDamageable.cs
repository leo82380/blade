using UnityEngine;

public enum DamageType
{
    Melee,
    Projectile,
    Range,
}
public interface IDamageable
{
    public void ApplyDamage(
        int damage, Vector3 hitPoint, Vector3 normal, float knockbackPower, 
        Agent dealer, DamageType damageType);
}
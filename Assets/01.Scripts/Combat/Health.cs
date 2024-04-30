using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDamageable
{
    public UnityEvent OnHitEvent;
    public UnityEvent OnDeadEvent;
    public ActionData actionData;
    
    private Agent _owner;
    protected int _currentHealth;

    public void Initialize(Agent agent)
    {
        _owner = agent;
        actionData = new ActionData();
        _currentHealth = _owner.Stat.maxHealth.GetValue();
    }
    public void ApplyDamage(int damage, Vector3 hitPoint, Vector3 normal, 
        float knockbackPower, Agent dealer, DamageType damageType)
    {
        if (_owner.Stat.CanEvasion())
        {
            Debug.Log("회피");
            return;
        }
        
        actionData.hitNormal = normal;
        actionData.hitPoint = hitPoint;
        actionData.lastDamageType = damageType;

        if (knockbackPower > 0)
        {
            ApplyKnockback(normal * -knockbackPower);
        }
        
        actionData.isCritical = dealer.Stat.IsCritical(ref damage);
        damage = _owner.Stat.ArmoredDamage(damage);
        
        _currentHealth = Mathf.Clamp(
            _currentHealth - damage, 0, _owner.Stat.maxHealth.GetValue());
        
        OnHitEvent?.Invoke();

        if (_currentHealth <= 0)
        {
            OnDeadEvent?.Invoke();
        }
    }

    private void ApplyKnockback(Vector3 force)
    {
        _owner.MovementCompo.GetKnockback(force);
    }
}

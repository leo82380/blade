using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDamageable
{
    public UnityEvent OnHitEvent;
    public ActionData actionData;
    
    private Agent _owner;

    public void Initialize(Agent agent)
    {
        _owner = agent;
        actionData = new ActionData();
    }
    public void ApplyDamage(int damage, Vector3 hitPoint, Vector3 normal, float knockbackPower, Agent dealer)
    {
        actionData.hitNormal = normal;
        actionData.hitPoint = hitPoint;

        if (knockbackPower > 0)
        {
            ApplyKnockback(normal * -knockbackPower);
        }
        
        OnHitEvent?.Invoke();
    }

    private void ApplyKnockback(Vector3 force)
    {
        _owner.MovementCompo.GetKnockback(force);
    }
}

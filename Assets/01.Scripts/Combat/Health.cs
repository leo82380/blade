using ObjectPooling;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDamageable
{
    public UnityEvent OnHitEvent;
    public UnityEvent OnDeadEvent;

    public ActionData actionData;

    private Agent _owner;
    private int _currentHealth;

    public void Initialize(Agent agent)
    {
        _owner = agent;
        actionData = new ActionData();
        _currentHealth = _owner.Stat.maxHealth.GetValue(); //ÃÖ´ëÃ¼·ÂÀ¸·Î ¼ÂÆÃ
    }
    
    public float GetNormalizedHealth()
    {
        return (float)_currentHealth / _owner.Stat.maxHealth.GetValue();
    }


    public void ApplyDamage(int damage, Vector3 hitPoint, Vector3 normal, float knockbackPower, Agent dealer, DamageType damageType)
    {
        if (_owner.isDead) return;

        Vector3 textPosition = hitPoint + new Vector3(0, 1f, 0);
        var popUp = PoolManager.Instance.Pop(PoolingType.PopUpText) as PopUpText;

        if(_owner.Stat.CanEvasion())
        {
            popUp.StartPopUp("Evasion!", textPosition, TextType.Message);
            return;
        }

        actionData.hitNormal = normal;
        actionData.hitPoint = hitPoint;
        actionData.lastDamageType = damageType;

        //³Ë¹éÀº ³ªÁß¿¡ ¿©±â¼­ Ã³¸®

        if (knockbackPower > 0)
        {
            ApplyKnockback(normal * -knockbackPower);
        }

        actionData.isCritical = dealer.Stat.IsCritical(ref damage);
        damage = _owner.Stat.ArmoredDamage(damage); //¾Æ¸Ó¼öÄ¡ Àû¿ëÇØ¼­ µ¥¹ÌÁö °è»ê

        _currentHealth = Mathf.Clamp(
                _currentHealth - damage, 0, _owner.Stat.maxHealth.GetValue());
        OnHitEvent?.Invoke();

        if(actionData.isCritical)
        {
            popUp.StartPopUp(damage.ToString(), textPosition, TextType.Critical);
        }
        else
        {
            popUp.StartPopUp(damage.ToString(), textPosition, TextType.Normal);
        }

        if(_currentHealth <= 0)
        {
            OnDeadEvent?.Invoke();
        }
    }

    private void ApplyKnockback(Vector3 force)
    {
        _owner.MovementCompo.GetKnockback(force);
    }
}

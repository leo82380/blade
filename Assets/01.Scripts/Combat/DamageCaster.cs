using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class DamageCaster : MonoBehaviour
{
    public LayerMask targetLayer;
    protected Agent _owner;
    
    public UnityEvent OnCastDamageEvent;

    public virtual void InitCaster(Agent agent)
    {
        _owner = agent;
    }
    
    public abstract bool CastDamage();
}

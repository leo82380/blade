using System;
using System.Collections;
using UnityEngine;

public abstract class Agent : MonoBehaviour
{
    #region Component list section
    public Animator AnimatorCompo { get; protected set; }
    public IMovement MovementCompo { get; protected set; }  
    public AgentVFX VFXCompo { get; protected set; }
    public DamageCaster DamageCasterCompo { get; protected set; }
    public Health HealthCompo { get; protected set; }
    #endregion
    
    [field:SerializeField] public AgentStat Stat {get; protected set;}
    
    public bool CanStateChangeable { get; protected set; } = true;
    public bool isDead;

    protected virtual void Awake()
    {
        Transform visualTrm = transform.Find("Visual");
        AnimatorCompo = visualTrm.GetComponent<Animator>();
        MovementCompo = GetComponent<IMovement>();
        MovementCompo.Initialize(this);

        VFXCompo = transform.Find("AgentVFX").GetComponent<AgentVFX>();
        
        Transform damageTrm = transform.Find("DamageCaster");
        if (damageTrm != null)
        {
            DamageCasterCompo = damageTrm.GetComponent<DamageCaster>();
            DamageCasterCompo.InitCaster(this);
        }

        Stat = Instantiate(Stat); // 자기 자신 복제본
        Stat.SetOwner(this);
        
        HealthCompo = GetComponent<Health>();
        HealthCompo.Initialize(this);
    }
    
    public Coroutine StartDelayCallback(float time, Action callback)
    {
        return StartCoroutine(DelayCoroutine(time, callback));
    }

    protected IEnumerator DelayCoroutine(float time, Action callback)
    {
        yield return new WaitForSeconds(time);
        callback?.Invoke();
    }

    public virtual void Attack()
    {
        //여기서는 아무것도 안함
    }
}

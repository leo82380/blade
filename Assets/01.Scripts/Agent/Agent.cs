using System;
using System.Collections;
using UnityEngine;

public abstract class Agent : MonoBehaviour
{
    #region 컴포넌트 리스트
    public Animator AnimatorCompo { get; protected set; }
    public IMovement MovementCompo { get; protected set; }  
    public AgentVFX VFXCompo { get; protected set; }
    #endregion
    
    public bool CanStateChangeable { get; protected set; } = true;
    public bool isDead;

    protected virtual void Awake()
    {
        Transform visualTrm = transform.Find("Visual");
        AnimatorCompo = visualTrm.GetComponent<Animator>();
        MovementCompo = GetComponent<IMovement>();
        MovementCompo.Initialize(this);

        VFXCompo = transform.Find("AgentVFX").GetComponent<AgentVFX>();
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

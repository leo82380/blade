using ObjectPooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EffectPlayer : PoolableMono
{
    [SerializeField] protected List<ParticleSystem> _particles;
    [SerializeField] protected List<VisualEffect> _effects;

    public void StartPlay(float time)
    {
        if (_particles != null)
            _particles.ForEach(p => p.Play());
        if (_effects != null)
            _effects.ForEach(e => e.Play());

        StartCoroutine(TimerCoroutine(time));
    }

    private IEnumerator TimerCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        PoolManager.Instance.Push(this);
    }
    
    public override void ResetItem()
    {
        if (_particles != null)
            _particles.ForEach(p => p.Simulate(0));
        if (_effects != null)
            _effects.ForEach(e => e.Stop());
    }
}
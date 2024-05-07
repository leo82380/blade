using System;
using System.Collections;
using System.Collections.Generic;
using ObjectPooling;
using UnityEngine;
using UnityEngine.VFX;

public class LaserTrail : PoolableMono
{
    private TrailRenderer _trailRenderer;
    [SerializeField] private VisualEffect _impactFlare;

    private void Awake()
    {
        _trailRenderer = GetComponent<TrailRenderer>();
    }

    public void DrawTrail(Vector3 start, Vector3 end, float lifeTime)
    {
        _trailRenderer.AddPosition(start);
        transform.position = end;
        
        _trailRenderer.time = lifeTime;
        
        _impactFlare.transform.position = end;
        _impactFlare.Play();
        
        StartCoroutine(LifeTimeCoroutine(lifeTime));
    }

    private IEnumerator LifeTimeCoroutine(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        PoolManager.Instance.Push(this);
    }

    public override void ResetItem()
    {
        _trailRenderer.Clear();
    }
}

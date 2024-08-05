using ObjectPooling;
using System.Collections;
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

    public void DrawTrail(Vector3 start, Vector3 end, float lifetime)
    {
        _trailRenderer.AddPosition(start);
        transform.position = end;

        _trailRenderer.time = lifetime;

        _impactFlare.transform.position = end;
        _impactFlare.Play();

        StartCoroutine(LifeTimeCoroutine(lifetime));
    }

    private IEnumerator LifeTimeCoroutine(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        PoolManager.Instance.Push(this);
    }

    public override void ResetItem()
    {
        _trailRenderer.Clear();
    }
}

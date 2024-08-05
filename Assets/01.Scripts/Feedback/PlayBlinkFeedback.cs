using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBlinkFeedback : Feedback
{
    [SerializeField] private SkinnedMeshRenderer _targetRenderer;
    [SerializeField] private float _blinkTime = 0.2f;

    
    private readonly int _blinkValueHash = Shader.PropertyToID("_BlinkValue");

    private Material _targetMaterial;
    private Coroutine _coroutine = null;

    private void Awake()
    {
        _targetMaterial = _targetRenderer.material;
    }
    public override void CreateFeedback()
    {
        _coroutine = StartCoroutine(BlinkCoroutine());
    }
    
    //고도 , 언리얼
    private IEnumerator BlinkCoroutine()
    {
        _targetMaterial.SetFloat(_blinkValueHash, 0.4f);
        yield return new WaitForSeconds(_blinkTime);
        _targetMaterial.SetFloat(_blinkValueHash, 0);
    }

    public override void FinishFeedback()
    {
        if(_coroutine != null)
            StopCoroutine(_coroutine);

        _targetMaterial.SetFloat(_blinkValueHash, 0);
    }
}

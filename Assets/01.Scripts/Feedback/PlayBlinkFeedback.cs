using System.Collections;
using UnityEngine;

public class PlayBlinkFeedback : Feedback
{
    [SerializeField] private SkinnedMeshRenderer _targetRenderer;
    [SerializeField] private float _blinkTime = 0.2f;
    
    private readonly int _blinkValueHash = Shader.PropertyToID("_BlinkValue");

    private Material _targetMaterial;
    private Coroutine _blinkCoroutine = null;
    private void Awake()
    {
        _targetMaterial = _targetRenderer.material;
    }
    
    public override void CreateFeedback()
    {
        _blinkCoroutine = StartCoroutine(BlinkCoroutine());
    }
    
    public override void FinishFeedback()
    {
        if (_blinkCoroutine != null)
            StopCoroutine(_blinkCoroutine);
        
        _targetMaterial.SetFloat(_blinkValueHash, 0);
    }

    private IEnumerator BlinkCoroutine()
    {
        _targetMaterial.SetFloat(_blinkValueHash, 0.4f);
        yield return new WaitForSeconds(_blinkTime);
        _targetMaterial.SetFloat(_blinkValueHash, 0);
    }
}

using ObjectPooling;
using UnityEngine;

public class PlaySplashEffectFeedback : Feedback
{
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] [ColorUsage(true, true)] private Color _bloodColor;
    [SerializeField] private float _playTime = 6f;
    [SerializeField] private DamageType _targetType;
    
    private float _yDelta = -2f;
    public override void CreateFeedback()
    {
        ActionData actionData = _owner.HealthCompo.actionData;
        if (actionData.lastDamageType != _targetType) return;
        
        var effect = PoolManager.Instance.Pop(PoolingType.VFX_Splash) as SplashEffectPlayer;
        effect.transform.position = actionData.hitPoint;
        
        if (Physics.Raycast(actionData.hitPoint, Vector3.down, out RaycastHit hit, 10f, _whatIsGround))
        {
            effect.SetCustomData(_bloodColor, -hit.distance);
        }
        effect.StartPlay(_playTime);
    }

    public override void FinishFeedback()
    {
    }
}

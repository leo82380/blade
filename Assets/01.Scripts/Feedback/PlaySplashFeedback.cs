using ObjectPooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySplashFeedback : Feedback
{
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] [ColorUsage(true, true)] private Color _bloodColor;
    [SerializeField] private DamageType _targetType;

    public override void CreateFeedback()
    {
        ActionData action = _owner.HealthCompo.actionData;
        if (action.lastDamageType != _targetType) return;

        var effect = PoolManager.Instance.Pop(PoolingType.VFX_Splash) as SplashEffectPlayer;

        effect.transform.position = action.hitPoint;

        if(Physics.Raycast(action.hitPoint, Vector3.down, out RaycastHit hit, 10f, _whatIsGround))
        {
            effect.SetCustomData(_bloodColor, -hit.distance);
        }
        effect.StartPlay(6f);

    }

    public override void FinishFeedback()
    {
        
    }
}

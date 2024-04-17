using System.Collections;
using System.Collections.Generic;
using ObjectPooling;
using UnityEngine;

public class PlaySplashEffectFeedback : Feedback
{
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] [ColorUsage(true, true)] private Color _bloodColor;
    [SerializeField] private float _playTime = 6f;
    
    private float _yDelta = -2f;
    public override void CreateFeedback()
    {
        var effect = PoolManager.Instance.Pop(PoolingType.VFX_Splash) as SplashEffectPlayer;
        ActionData actionData = _owner.HealthCompo.actionData;
        RaycastHit hit;
        if (Physics.Raycast(actionData.hitPoint, Vector3.down, out hit, 10f, _whatIsGround))
        {
            effect.transform.position = hit.point;
            _yDelta = transform.position.y - hit.point.y;
            effect.SetCustomData(_bloodColor, -_yDelta);
        }
        else
        {
            effect.transform.position = actionData.hitPoint;
            effect.SetCustomData(_bloodColor, -2f);
        }
        effect.StartPlay(_playTime);
    }

    public override void FinishFeedback()
    {
    }
}

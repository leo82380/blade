using UnityEngine;
using ObjectPooling;

public class PlaySlashEffectFeedback : Feedback
{
    [SerializeField] private float _playTime = 0.8f;
    public override void CreateFeedback()
    {
        var effect = PoolManager.Instance.Pop(PoolingType.VFX_Slash) as EffectPlayer;
        
        ActionData actionData = _owner.HealthCompo.actionData;
        
        effect.transform.position = actionData.hitPoint;
        effect.StartPlay(_playTime);
    }

    public override void FinishFeedback()
    {
    }
}

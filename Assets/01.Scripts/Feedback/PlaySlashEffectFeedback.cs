using ObjectPooling;
using UnityEngine;

public class PlaySlashEffectFeedback : Feedback
{
    [SerializeField] private float _playTime = 0.8f;
    [SerializeField] private DamageType _targetType;

    public override void CreateFeedback()
    {
        ActionData actionData = _owner.HealthCompo.actionData;
        if (actionData.lastDamageType != _targetType) return;

        var effect = PoolManager.Instance.Pop(PoolingType.VFX_Slash) as EffectPlayer;

        effect.transform.position = actionData.hitPoint;
        effect.StartPlay(_playTime);
    }

    public override void FinishFeedback()
    {

    }
}

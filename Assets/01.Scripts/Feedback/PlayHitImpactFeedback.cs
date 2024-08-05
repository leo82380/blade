using ObjectPooling;
using UnityEngine;

public class PlayHitImpactFeedback : Feedback
{
    [SerializeField] private float _playTime = 1.5f;
    public override void CreateFeedback()
    {
        var effect = PoolManager.Instance.Pop(PoolingType.VFX_Hit) as EffectPlayer;
        ActionData actionData = _owner.HealthCompo.actionData;

        Quaternion rot = Quaternion.LookRotation(actionData.hitNormal * -1);
        effect.transform.SetPositionAndRotation(actionData.hitPoint, rot);
        effect.StartPlay(_playTime);
    }

    public override void FinishFeedback()
    {

    }
}

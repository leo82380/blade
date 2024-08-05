using ObjectPooling;
using UnityEngine;

public class PlayEffectFeedback : Feedback
{
    [SerializeField] private float _playTime;
    [SerializeField] private PoolingType _type;
    public override void CreateFeedback()
    {
        EffectPlayer ep = PoolManager.Instance.Pop(_type) as EffectPlayer;
        ep.transform.position = transform.position;
        ep.StartPlay(_playTime);
    }

    public override void FinishFeedback()
    {
        
    }
}
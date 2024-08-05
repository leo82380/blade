using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineImpulseSource))]
public class ImpulseFeedback : Feedback
{
    private CinemachineImpulseSource _source;
    [SerializeField] private float _impulsePower = 1f;

    protected override void Awake()
    {
        base.Awake();
        _source = GetComponent<CinemachineImpulseSource>();
    }

    public override void CreateFeedback()
    {
        _source.GenerateImpulse(_impulsePower);
    }

    public override void FinishFeedback()
    {
        
    }
}
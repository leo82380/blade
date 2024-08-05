using UnityEngine;
using UnityEngine.VFX;

public class EnemyVFX : AgentVFX
{
    [SerializeField] private VisualEffect _footStep;

    public void PlayBurstFootStep()
    {
        _footStep.Play();
    }
}

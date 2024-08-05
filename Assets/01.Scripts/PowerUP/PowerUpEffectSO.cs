using UnityEngine;

public enum EffectType
{
    StatInc,
    SkillUnlock,
    SkillUpgrade
}
public abstract class PowerUpEffectSO : ScriptableObject
{
    public string code;
    public EffectType type;

    public abstract void UseEffect();
    public abstract bool CanUpgradeEffect();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUpEffectSO : ScriptableObject
{
    public abstract void UseEffect();
    public abstract bool CanUpgradeEffect();
}

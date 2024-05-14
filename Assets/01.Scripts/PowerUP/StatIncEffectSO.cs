using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/PowerUp/Effect/StatInc")]
public class StatIncEffectSO : PowerUpEffectSO
{
    public StatType targetStat;
    public int increaseValue;
    
    public override bool CanUpgradeEffect()
    {
        return true;
    }
    
    public override void UseEffect()
    {
        PlayerManager.Instance.Player.Stat.AddModifier(targetStat, increaseValue);
    }

    
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/PowerUp/Data")]
public class PowerUpSO : ScriptableObject
{
    public string code;
    public PlayerSkill shouldBeUnlock;
    public string title;
    [TextArea] public string description;
    public Sprite icon;

    public List<PowerUpEffectSO> effectList;

    public bool CheckCanUpgrade()
    {
        if (shouldBeUnlock != PlayerSkill.None)
        {
            Skill skill = SkillManager.Instance.GetSkill(shouldBeUnlock);
            if (skill.skillEnabled == false) return false;
        }

        if (effectList.Any(e => e.CanUpgradeEffect() == false))
            return false;
        
        return true;
    }
}

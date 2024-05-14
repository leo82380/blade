using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/PowerUp/Effect/UnLockSkill")]
public class UnLockSkillEffectSO : PowerUpEffectSO
{
    public PlayerSkill unLockSkill;
    
    public override bool CanUpgradeEffect()
    {
        Skill skill = SkillManager.Instance.GetSkill(unLockSkill);
        return skill.skillEnabled == false;
    }
    public override void UseEffect()
    {
        Skill skill = SkillManager.Instance.GetSkill(unLockSkill);
        skill.UnlockSkill();
    }

    
}

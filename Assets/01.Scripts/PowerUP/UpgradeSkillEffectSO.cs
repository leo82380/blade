using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/PowerUp/Effect/UpgradeSkill")]
public class UpgradeSkillEffectSO : PowerUpEffectSO
{
    public enum SkillUpgradeType
    {
        ByField,
        ByMethod
    }

    
    public PlayerSkill targetSkill;
    public SkillUpgradeType upgradeType = SkillUpgradeType.ByField;
    
    public bool isFloat = true;
    public float floatValue;
    public int intValue;
    public int selectFieldIndex;
    public int selectMethodIndex;
    public string callParams;
    
    public List<FieldInfo> fieldList = new List<FieldInfo>();
    public List<MethodInfo> methodList = new List<MethodInfo>();
    
    private MethodInfo canUpgradeMethod;

    public object[] methodCallParamArray;
    
    public override void UseEffect()
    {
        if (upgradeType == SkillUpgradeType.ByField)
        {
            FieldUpgrade();
        }
        else if (upgradeType == SkillUpgradeType.ByMethod)
        {
            MethodUpgrade();
        }
    }

    private void FieldUpgrade()
    {
        FieldInfo field = fieldList[selectFieldIndex];
        Skill skill = SkillManager.Instance.GetSkill(targetSkill);
        
        if (isFloat)
        {
            float value = (float)field.GetValue(skill);
            field.SetValue(skill, value + floatValue);
        }
        else
        {
            int value = (int)field.GetValue(skill);
            field.SetValue(skill, value + intValue);
        }
    }

    private void MethodUpgrade()
    {
        MethodInfo method = methodList[selectMethodIndex];
        Skill skill = SkillManager.Instance.GetSkill(targetSkill);
        
        method.Invoke(skill, methodCallParamArray);
    }

    public override bool CanUpgradeEffect()
    {
        if (upgradeType == SkillUpgradeType.ByField)
            return true;
        if (upgradeType == SkillUpgradeType.ByMethod)
        {
            Skill skill = SkillManager.Instance.GetSkill(targetSkill);
            return (bool) canUpgradeMethod?.Invoke(skill, null);
        }
        return false;
    }
    
}

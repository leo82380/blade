using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        else if(upgradeType == SkillUpgradeType.ByMethod)
        {
            MethodUpgrade();
        }
    }

    private void FieldUpgrade()
    {
        FieldInfo field = fieldList[selectFieldIndex];
        Skill skill = SkillManager.Instance.GetSkill(targetSkill);

        if(isFloat)
        {
            float value = (float)field.GetValue(skill); //해당 스킬에서 해당 필드값
            field.SetValue(skill, value + floatValue);
        }
        else
        {
            int value = (int)field.GetValue(skill);
            field.SetValue(skill, intValue + value);
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

        if(upgradeType == SkillUpgradeType.ByMethod)
        {
            Skill skill = SkillManager.Instance.GetSkill(targetSkill);
            return (bool)canUpgradeMethod?.Invoke(skill, null);
        }

        return false;
    }

    private void OnEnable()
    {
        GetReflectionInfo();
    }

    private void GetReflectionInfo()
    {
        fieldList.Clear();
        methodList.Clear();

        if (targetSkill == 0) return; //타겟스킬이 아직 활성화되어있지 않다면 리턴해라.

        Type t = Type.GetType($"{targetSkill}Skill"); //스킬 클래스가 가져와진다.
        FieldInfo[] fields = t.GetFields(BindingFlags.Instance | BindingFlags.Public);

        foreach(FieldInfo f in fields)
        {
            if(f.FieldType == typeof(int) || f.FieldType == typeof(float))
            {
                fieldList.Add(f); //int 나 float 변수타입만 가져온다.
            }
        }

        MethodInfo[] methods = t.GetMethods(BindingFlags.Instance | BindingFlags.Public);

        // true, 1, "abc"
        methodCallParamArray = callParams.Split(
            new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => GetDataFromString(x)).ToArray();

        foreach(MethodInfo m in methods)
        {
            if (m.Name.StartsWith("Upgrade"))
                methodList.Add(m);
        }

        
        if (methodList.Count > selectMethodIndex && selectMethodIndex >= 0)
        {
            MethodInfo selectedMethod = methodList[selectMethodIndex];
            if (selectedMethod != null)
                canUpgradeMethod = t.GetMethod($"Can{selectedMethod.Name}");
            else
                Debug.LogWarning($"There is no check method for {selectedMethod.Name}");
        }
        else
        {
            selectMethodIndex = -1;
        }

        type = EffectType.SkillUpgrade;
    }

    private object GetDataFromString(string strInput)
    {
        object data;
        if (strInput.StartsWith("\""))
            data = strInput.Trim('\"');
        else if (bool.TryParse(strInput, out bool bTemp))
            data = bTemp;
        else if (int.TryParse(strInput, out int iTemp))
            data = iTemp;
        else if (float.TryParse(strInput, out float fTemp))
            data = fTemp;
        else
            data = strInput;

        return data;
    }

    private void OnValidate()
    {
        GetReflectionInfo();
    }
}

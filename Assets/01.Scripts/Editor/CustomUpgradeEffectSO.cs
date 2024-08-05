using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UpgradeSkillEffectSO))]
public class CustomUpgradeEffectSO : CustomPowerEffectSO
{
    private SerializedProperty targetSkillProp;
    private SerializedProperty upgradeTypeProp;

    protected override void OnEnable()
    {
        base.OnEnable();
        targetSkillProp = serializedObject.FindProperty("targetSkill");
        upgradeTypeProp = serializedObject.FindProperty("upgradeType");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        try
        {
            serializedObject.Update();
            UpgradeSkillEffectSO so = (UpgradeSkillEffectSO)target;

            EditorGUILayout.PropertyField(targetSkillProp);
            EditorGUILayout.PropertyField(upgradeTypeProp);

            if(targetSkillProp.enumValueIndex == 0)   //선택된 스킬이 0번
            {
                return;
            }

            if(so.upgradeType == UpgradeSkillEffectSO.SkillUpgradeType.ByField)
            {
                DrawFields(so);
            }else if(so.upgradeType == UpgradeSkillEffectSO.SkillUpgradeType.ByMethod)
            {
                DrawMethods(so);
            }

            serializedObject.ApplyModifiedProperties();
        }catch(Exception e)
        {
            Debug.LogWarning($"Error occur : {e.Message}");
        }
    }

    private void DrawFields(UpgradeSkillEffectSO so)
    {
        GUIContent fieldLabel = new GUIContent("Target field select");
        string[] names = so.fieldList.Select(x => x.Name).ToArray();
        if (names.Length == 0) return;

        if(names.Length <= so.selectFieldIndex)
        {
            so.selectFieldIndex = 0;
        }

        so.selectFieldIndex = EditorGUILayout.Popup(fieldLabel, so.selectFieldIndex, names);

        FieldInfo field = so.fieldList[so.selectFieldIndex];

        bool isError = false;
        if (field.FieldType == typeof(int))
            so.isFloat = false;
        else if (field.FieldType == typeof(float))
            so.isFloat = true;
        else
            isError = true;

        if(isError)
        {
            EditorGUILayout.LabelField("Error. Can't add or sub to this value");
        }else
        {
            if(so.isFloat)
            {
                so.floatValue = EditorGUILayout.FloatField("Inc(or Dec) Float", so.floatValue);
            }
            else
            {
                so.intValue = EditorGUILayout.IntField("Inc(or Dec) Int", so.intValue);
            }
        }
    }

    private void DrawMethods(UpgradeSkillEffectSO so)
    {
        GUIContent methodLabel = new GUIContent("Target method select");
        string[] names = so.methodList.Select(x => x.Name).ToArray();

        if (names.Length == 0) return;

        if (names.Length <= so.selectMethodIndex)
            so.selectMethodIndex = 0;

        if(names.Length > 0)
        {
            so.selectMethodIndex = EditorGUILayout.Popup(methodLabel, so.selectMethodIndex, names);
            so.callParams = EditorGUILayout.TextField("Method Param", so.callParams);
        }
    }

}

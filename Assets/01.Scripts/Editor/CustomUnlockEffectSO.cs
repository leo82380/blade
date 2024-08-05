using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UnLockSkillEffectSO))]
public class CustomUnlockEffectSO : CustomPowerEffectSO
{
    private SerializedProperty unLockSkillProp;

    protected override void OnEnable()
    {
        base.OnEnable();
        unLockSkillProp = serializedObject.FindProperty("unLockSkill");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        try
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(unLockSkillProp);
            serializedObject.ApplyModifiedProperties();
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"error occur when draw : {ex.Message}");
        }
    }
}

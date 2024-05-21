using UnityEditor;
using UnityEngine;
using System;

[CustomEditor(typeof(UnLockSkillEffectSO))]
public class CustomUnlockEffectSO : CustomPowerUpEffectSO
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
        catch (Exception e)
        {
            Debug.Log($"exception occur when draw : {e.Message}");
        }
    }
}

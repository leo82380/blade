using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StatIncEffectSO))]
public class CustomStatIncEffectSO : CustomPowerEffectSO
{
    private SerializedProperty targetStatProp;
    private SerializedProperty increaseValueProp;

    protected override void OnEnable()
    {
        base.OnEnable();
        targetStatProp = serializedObject.FindProperty("targetStat");
        increaseValueProp = serializedObject.FindProperty("increaseValue");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        try
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(targetStatProp);
            EditorGUILayout.PropertyField(increaseValueProp);

            serializedObject.ApplyModifiedProperties();
        }
        catch(Exception ex)
        {
            Debug.Log($"exception occure when draw : {ex.Message}");
        }
    }
}

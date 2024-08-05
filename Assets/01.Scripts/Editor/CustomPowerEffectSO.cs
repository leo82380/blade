using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PowerUpEffectSO), true)] //�ڽ� Ŭ�������� �����Ͽ� �����Ѵ�.
public class CustomPowerEffectSO : Editor
{
    private SerializedProperty codeProp;
    private SerializedProperty typeProp;

    protected virtual void OnEnable()
    {
        GUIUtility.keyboardControl = 0;
        codeProp = serializedObject.FindProperty("code");
        typeProp = serializedObject.FindProperty("type");
    }

    public override void OnInspectorGUI()
    {
        //�����Ҷ� �� �ؾ��ϴ°�
        serializedObject.Update();

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUI.BeginChangeCheck();
            string prevName = codeProp.stringValue;
            EditorGUILayout.PrefixLabel("Code");
            EditorGUILayout.DelayedTextField(codeProp, GUIContent.none);

            if(EditorGUI.EndChangeCheck())
            {
                string assetPath = AssetDatabase.GetAssetPath(target);
                string newName = $"Effect_{codeProp.stringValue}";
                serializedObject.ApplyModifiedProperties();

                string msg = AssetDatabase.RenameAsset(assetPath, newName);
                if(string.IsNullOrEmpty(msg))
                {
                    target.name = newName;
                    EditorGUILayout.EndHorizontal();
                    return;
                }
                codeProp.stringValue = prevName;
            }
        }
        EditorGUILayout.EndHorizontal();

        GUI.enabled = false; //���� ������ �������°�.
        EditorGUILayout.PropertyField(typeProp);
        GUI.enabled = true;

        //������ �� �ؾ��ϴ°�
        serializedObject.ApplyModifiedProperties();
    }
}

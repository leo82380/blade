using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PowerUpEffectSO), true)] //자식 클래스까지 포함하여 적용한다.
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
        //시작할때 꼭 해야하는것
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

        GUI.enabled = false; //변수 편집을 꺼버리는거.
        EditorGUILayout.PropertyField(typeProp);
        GUI.enabled = true;

        //끝날때 꼭 해야하는것
        serializedObject.ApplyModifiedProperties();
    }
}

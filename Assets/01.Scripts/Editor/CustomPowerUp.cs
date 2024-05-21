using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PowerUpSO))]
public class CustomPowerUp : Editor
{
    private SerializedProperty codeProp;
    private SerializedProperty shouldBeUnlockProp;
    private SerializedProperty titleProp;
    private SerializedProperty descProp;
    private SerializedProperty iconProp;

    public SerializedProperty effectListProp;

    private GUIStyle _textAreaStyle; //텍스트 랩 스타일을 지정하기 위해서 


    private void OnEnable()
    {
        //왜했는지 기억나니? 텍스트 입력에 포커스 
        GUIUtility.keyboardControl = 0;
        codeProp = serializedObject.FindProperty("code");
        shouldBeUnlockProp = serializedObject.FindProperty("shouldBeUnlock");
        titleProp = serializedObject.FindProperty("title");
        descProp = serializedObject.FindProperty("description");
        iconProp = serializedObject.FindProperty("icon");

        effectListProp = serializedObject.FindProperty("effectList");
    }

    private void StyleSetUp()
    {
        if (_textAreaStyle == null)
        {
            _textAreaStyle = new GUIStyle(EditorStyles.textArea);
            _textAreaStyle.wordWrap = true; //이것때문에 오버라이드 한다.
        }
    }

    public override void OnInspectorGUI()
    {
        StyleSetUp();
        //시작할때 해줄 일
        serializedObject.Update();

        EditorGUILayout.Space(10f);
        EditorGUILayout.BeginHorizontal("HelpBox");
        {
            iconProp.objectReferenceValue = EditorGUILayout.ObjectField(GUIContent.none,
                iconProp.objectReferenceValue,
                typeof(Sprite),
                false,
                GUILayout.Width(65));

            EditorGUILayout.BeginVertical();
            {

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUI.BeginChangeCheck();
                    string prevName = codeProp.stringValue;
                    EditorGUILayout.PrefixLabel("Code");
                    EditorGUILayout.DelayedTextField(codeProp, GUIContent.none);

                    if (EditorGUI.EndChangeCheck())
                    {
                        string assetPath = AssetDatabase.GetAssetPath(target);
                        string newName = $"PowerUp_{codeProp.stringValue}";

                        serializedObject.ApplyModifiedProperties();

                        string msg = AssetDatabase.RenameAsset(assetPath, newName);

                        //성공적으로 이름을 변경했다면 null이 리턴된다.
                        if (string.IsNullOrEmpty(msg))
                        {
                            target.name = newName;

                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.EndVertical();
                            EditorGUILayout.EndHorizontal();
                            return;
                        }
                        codeProp.stringValue = prevName;
                    }
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.PropertyField(shouldBeUnlockProp);
                EditorGUILayout.PropertyField(titleProp);
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.PropertyField(descProp);
        EditorGUILayout.PropertyField(effectListProp);

        //끝날 때 해줄일 
        serializedObject.ApplyModifiedProperties();
    }
}
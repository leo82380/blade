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

    private GUIStyle _textAreaStyle; //�ؽ�Ʈ �� ��Ÿ���� �����ϱ� ���ؼ� 


    private void OnEnable()
    {
        //���ߴ��� ��ﳪ��? �ؽ�Ʈ �Է¿� ��Ŀ�� 
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
            _textAreaStyle.wordWrap = true; //�̰Ͷ����� �������̵� �Ѵ�.
        }
    }

    public override void OnInspectorGUI()
    {
        StyleSetUp();
        //�����Ҷ� ���� ��
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

                        //���������� �̸��� �����ߴٸ� null�� ���ϵȴ�.
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

        //���� �� ������ 
        serializedObject.ApplyModifiedProperties();
    }
}

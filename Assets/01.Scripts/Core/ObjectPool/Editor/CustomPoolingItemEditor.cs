using ObjectPooling;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PoolingItemSO))]
public class CustomPoolingItemEditor : Editor
{
    private SerializedProperty enumNameProp;
    private SerializedProperty poolingNameProp;
    private SerializedProperty descriptionProp;
    private SerializedProperty poolCountProp;
    private SerializedProperty prefabProp;


    private GUIStyle textAreaStyle;

    private void OnEnable()
    {
        GUIUtility.keyboardControl = 0;

        enumNameProp = serializedObject.FindProperty("enumName");
        poolingNameProp = serializedObject.FindProperty("poolingName");
        descriptionProp = serializedObject.FindProperty("description");
        poolCountProp = serializedObject.FindProperty("poolCount");
        prefabProp = serializedObject.FindProperty("prefab");
    }

    private void StyleSetup()
    {
        if (textAreaStyle == null)
        {
            textAreaStyle = new GUIStyle(EditorStyles.textArea);
            textAreaStyle.wordWrap = true;
        }
    }

    public override void OnInspectorGUI()
    {
        StyleSetup();

        serializedObject.Update();

        EditorGUILayout.BeginHorizontal("HelpBox");
        {
            EditorGUILayout.BeginVertical();
            {

                EditorGUI.BeginChangeCheck(); //변경을 체크한다.
                string prevName = enumNameProp.stringValue;
                //엔터가 쳐지거나 포커스가 나갈때까지 변경을 저장하지 않아.
                EditorGUILayout.DelayedTextField(enumNameProp);

                if (EditorGUI.EndChangeCheck())
                {
                    //현재 편집중인 에셋의 경로를 알아내라.
                    string assetPath = AssetDatabase.GetAssetPath(target);
                    string newName = $"Pool_{enumNameProp.stringValue}";
                    serializedObject.ApplyModifiedProperties();

                    string msg = AssetDatabase.RenameAsset(assetPath, newName);

                    //성공적으로 파일명 변경했어요.
                    if (string.IsNullOrEmpty(msg))
                    {
                        target.name = newName;
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndHorizontal();
                        return;
                    }
                    enumNameProp.stringValue = prevName;
                }


                EditorGUILayout.PropertyField(poolingNameProp);

                EditorGUILayout.BeginVertical("HelpBox");
                {
                    EditorGUILayout.LabelField("Description");

                    descriptionProp.stringValue = EditorGUILayout.TextArea(
                        descriptionProp.stringValue,
                        textAreaStyle,
                        GUILayout.Height(60)); 
                }
                EditorGUILayout.EndVertical();


                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.PrefixLabel("PoolSettings");
                    EditorGUILayout.PropertyField(poolCountProp, GUIContent.none);
                    EditorGUILayout.PropertyField(prefabProp, GUIContent.none);
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();


        serializedObject.ApplyModifiedProperties();
    }
}

using System;
using UnityEditor;
using UnityEngine;

public enum UtilType
{
    Pool,
    PowerUp,
    Effect
}

public class UtilityWindow : EditorWindow
{
    private static int toolbarIndex = 0;

    private string[] _toolbarItemNames;

    #region 각 데이터 테이블 모음
    private readonly string _poolDirectory = "Assets/08.SO/ObjectPool";
    private PoolingTableSO _poolTable;
    #endregion
    
    [MenuItem("Tools/Utility")]
    private static void OpenWindow()
    {
        UtilityWindow window = GetWindow<UtilityWindow>("Utility");
        window.minSize = new Vector2(700, 500);
        window.Show();
    }

    private void OnEnable()
    {
        SetUpUtility();
    }

    private void SetUpUtility()
    {
        // int enumLength = Enum.GetNames(typeof(UtilType)).Length;
        // _toolbarItemNames = new string[enumLength];
        // int i = 0;
        // foreach (UtilType type in Enum.GetValues(typeof(UtilType)))
        // {
        //     _toolbarItemNames[i] = type.ToString();
        //     i++;
        // }
        _toolbarItemNames = Enum.GetNames(typeof(UtilType));

        if (_poolTable == null)
        {
            _poolTable = AssetDatabase.LoadAssetAtPath<PoolingTableSO>
                ($"{_poolDirectory}/table.asset");
            if (_poolTable == null)
            {
                _poolTable = CreateInstance<PoolingTableSO>();
                string fileName = AssetDatabase.GenerateUniqueAssetPath
                    ($"{_poolDirectory}/table.asset");
                
                AssetDatabase.CreateAsset(_poolTable, fileName);
                Debug.Log($"Create Pooling Table at {fileName}");
            }
        }
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void OnGUI()
    {
        toolbarIndex = GUILayout.Toolbar(toolbarIndex, _toolbarItemNames);
        EditorGUILayout.Space(5f);

        DrawContent(toolbarIndex);
    }

    private void DrawContent(int toolbarIndex)
    {
        switch (toolbarIndex)
        {
            case 0:
                DrawPoolItems();
                break;
        }
    }

    private void DrawPoolItems()
    {
    }
}

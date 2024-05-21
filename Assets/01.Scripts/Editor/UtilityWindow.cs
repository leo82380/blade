using System.Collections.Generic;
using ObjectPooling;
using UnityEditor;
using UnityEngine;
using System.Text;
using System.IO;
using System;
using Object = UnityEngine.Object;

public enum UtilType
{
    Pool,
    PowerUp,
    Effect
}

public class UtilityWindow : EditorWindow
{
    private static int toolbarIndex = 0;
    private static Dictionary<UtilType, Vector2> scrollPositions 
        = new Dictionary<UtilType, Vector2>();
    private static Dictionary<UtilType, Object> selectedItem 
        = new Dictionary<UtilType, Object>();
    private static Vector2 inspectorScroll = Vector2.zero;

    private string[] _toolbarItemNames;
    private Editor _cachedEditor;
    private Texture2D _selectTexture;
    private GUIStyle _selectStyle;

    #region 각 데이터 테이블 모음
    private readonly string _poolDirectory = "Assets/08.SO/ObjectPool";
    private PoolingTableSO _poolTable;
    
    private readonly string _powerUpDirectory = "Assets/08.SO/PowerUp";
    private PowerUpListSO _powerUpTable;
    
    private readonly string _effectDirectory = "Assets/08.SO/PowerUp/Effects";
    private PowerUpEffectListSO _effectTable;
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

    private void OnDisable()
    {
        DestroyImmediate(_cachedEditor);
        DestroyImmediate(_selectTexture);
    }

    private void SetUpUtility()
    {
        _selectTexture = new Texture2D(1, 1); // 1픽셀짜리 텍스쳐 그림
        _selectTexture.SetPixel(0, 0, new Color(0.24f, 0.48f, 0.9f, 0.4f));
        _selectTexture.Apply();

        _selectStyle = new GUIStyle();
        _selectStyle.normal.background = _selectTexture;
        
        _selectTexture.hideFlags = HideFlags.DontSave;
        
        _toolbarItemNames = Enum.GetNames(typeof(UtilType));
        foreach (UtilType type in Enum.GetValues(typeof(UtilType)))
        {
            if (scrollPositions.ContainsKey(type) == false)
                scrollPositions[type] = Vector2.zero;
            if (selectedItem.ContainsKey(type) == false)
                selectedItem[type] = null;
        }

        if (_poolTable == null)
        {
            _poolTable = CreateAssetTable<PoolingTableSO>(_poolDirectory);
        }
        
        if (_powerUpTable == null)
        {
            _powerUpTable = CreateAssetTable<PowerUpListSO>(_powerUpDirectory);
        }
        
        if (_effectTable == null)
        {
            _effectTable = CreateAssetTable<PowerUpEffectListSO>(_effectDirectory);
        }
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private T CreateAssetTable<T>(string path) where T : ScriptableObject
    {
        T table = AssetDatabase.LoadAssetAtPath<T>
            ($"{path}/table.asset");
        if (table == null)
        {
            table = ScriptableObject.CreateInstance<T>();
            string fileName = AssetDatabase.GenerateUniqueAssetPath
                ($"{path}/table.asset");
                
            AssetDatabase.CreateAsset(table, fileName);
            Debug.Log($"{typeof(T).Name} Table Create at {fileName}");
        }

        return table;
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
            case 1:
                DrawPowerUpItems();
                break;
            case 2:
                DrawEffectItems();
                break;
        }
    }
    
    private T GenerateEffectAsset<T>(string path) where T : PowerUpEffectSO
    {
        Guid guid = Guid.NewGuid();
        T newData = CreateInstance<T>();
        newData.code = guid.ToString();
        AssetDatabase.CreateAsset(newData, $"{path}/Effect_{guid}.asset");
        _effectTable.list.Add(newData);
        EditorUtility.SetDirty(_effectTable);
        AssetDatabase.SaveAssets();
        
        return newData;
    }

    private void DrawEffectItems()
    {
        // 스킬 증가, 스킬 언락, 스킬업그레이드
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Stat inc"))
            {
                GenerateEffectAsset<StatIncEffectSO>($"{_effectDirectory}/StatInc");
            }

            if (GUILayout.Button("Skill Unlock"))
            {
                GenerateEffectAsset<UnLockSkillEffectSO>($"{_effectDirectory}/UnlockSkill");
            }

            if (GUILayout.Button("Skill Upgrade"))
            {
                GenerateEffectAsset<UpgradeSkillEffectSO>($"{_effectDirectory}/UpgradeSkill");
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(300f));
            {
                EditorGUILayout.LabelField("Effect list");
                EditorGUILayout.Space(3f);


                scrollPositions[UtilType.Effect] = EditorGUILayout.BeginScrollView
                (scrollPositions[UtilType.Effect], false, true,
                    GUIStyle.none, GUI.skin.verticalScrollbar, GUIStyle.none);
                {
                    foreach (var so in _effectTable.list)
                    {
                        float labelWith = 220f;
                        GUIStyle style = selectedItem[UtilType.Effect] == so
                            ? _selectStyle
                            : GUIStyle.none;

                        // 한줄 그린다.
                        EditorGUILayout.BeginHorizontal(style, GUILayout.Height(40f));
                        {
                            EditorGUILayout.LabelField(
                                $"[{so.type}]", GUILayout.Width(60f), GUILayout.Height(40f));
                            EditorGUILayout.LabelField(
                                $"[{so.code}]", GUILayout.Width(labelWith), GUILayout.Height(40f));

                            EditorGUILayout.BeginVertical();
                            {
                                EditorGUILayout.Space(10f);
                                GUI.color = Color.red;
                                if (GUILayout.Button("X", GUILayout.Width(20f)))
                                {
                                    _effectTable.list.Remove(so);
                                    AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(so));
                                    EditorUtility.SetDirty(_effectTable);
                                    AssetDatabase.SaveAssets();
                                }

                                GUI.color = Color.white;
                            }
                            EditorGUILayout.EndVertical();
                            // End of Delete
                        }
                        EditorGUILayout.EndHorizontal();

                        if (so == null)
                            break;

                        // 마지막으로 그린 사각형 정보를 알아옴
                        Rect lastRect = GUILayoutUtility.GetLastRect();

                        if (Event.current.type == EventType.MouseDown
                            && lastRect.Contains(Event.current.mousePosition))
                        {
                            inspectorScroll = Vector2.zero;
                            selectedItem[UtilType.Effect] = so;
                            Event.current.Use();
                        }
                    }
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
            if (selectedItem[UtilType.Effect] != null)
            {
                inspectorScroll = EditorGUILayout.BeginScrollView(inspectorScroll);
                {
                    EditorGUILayout.Space(2f);
                    Editor.CreateCachedEditor(
                        selectedItem[UtilType.Effect], null, ref _cachedEditor);
                        
                    _cachedEditor.OnInspectorGUI();
                }
                EditorGUILayout.EndScrollView();
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    private void DrawPowerUpItems()
    {
        EditorGUILayout.BeginHorizontal();
        {
            GUI.color = new Color(0.19f, 0.76f, 0.08f);
            if(GUILayout.Button("New PowerUp Item"))
            {
                Guid guid = Guid.NewGuid();
                PowerUpSO newData = CreateInstance<PowerUpSO>();
                newData.code = guid.ToString();
                AssetDatabase.CreateAsset(
                    newData, $"{_powerUpDirectory}/PowerUp_{newData.code}.asset");
                _powerUpTable.list.Add(newData);
                
                EditorUtility.SetDirty(_powerUpTable);
                AssetDatabase.SaveAssets();
            }
        }
        EditorGUILayout.EndHorizontal();
        GUI.color = Color.white;

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(300f));
            {
                EditorGUILayout.LabelField("PowerUp list");
                EditorGUILayout.Space(3f);


                scrollPositions[UtilType.PowerUp] = EditorGUILayout.BeginScrollView
                (scrollPositions[UtilType.PowerUp], false, true,
                    GUIStyle.none, GUI.skin.verticalScrollbar, GUIStyle.none);
                {
                    foreach (var so in _powerUpTable.list)
                    {
                        float labelWith = so.icon != null ? 200f : 240f;
                        GUIStyle style = selectedItem[UtilType.PowerUp] == so
                            ? _selectStyle
                            : GUIStyle.none;

                        // 한줄 그린다.
                        EditorGUILayout.BeginHorizontal(style, GUILayout.Height(40f));
                        {
                            if (so.icon != null)
                            {
                                // 아이콘 그리기
                                Texture2D previewTexture = AssetPreview.GetAssetPreview(so.icon);
                                GUILayout.Label(previewTexture, GUILayout.Width(40f), GUILayout.Height(40f));
                            }

                            EditorGUILayout.LabelField(so.code, GUILayout.Width(labelWith), GUILayout.Height(40f));

                            EditorGUILayout.BeginVertical();
                            {
                                EditorGUILayout.Space(10f);
                                GUI.color = Color.red;
                                if (GUILayout.Button("X", GUILayout.Width(20f)))
                                {
                                    _powerUpTable.list.Remove(so);
                                    AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(so));
                                    EditorUtility.SetDirty(_powerUpTable);
                                    AssetDatabase.SaveAssets();
                                }

                                GUI.color = Color.white;
                            }
                            EditorGUILayout.EndVertical();
                            // End of Delete
                        }
                        EditorGUILayout.EndHorizontal();

                        if (so == null)
                            break;

                        // 마지막으로 그린 사각형 정보를 알아옴
                        Rect lastRect = GUILayoutUtility.GetLastRect();

                        if (Event.current.type == EventType.MouseDown
                            && lastRect.Contains(Event.current.mousePosition))
                        {
                            inspectorScroll = Vector2.zero;
                            selectedItem[UtilType.PowerUp] = so;
                            Event.current.Use();
                        }
                    }
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
            if (selectedItem[UtilType.PowerUp] != null)
            {
                inspectorScroll = EditorGUILayout.BeginScrollView(inspectorScroll);
                {
                    EditorGUILayout.Space(2f);
                    Editor.CreateCachedEditor(
                        selectedItem[UtilType.PowerUp], null, ref _cachedEditor);
                        
                    _cachedEditor.OnInspectorGUI();
                }
                EditorGUILayout.EndScrollView();
            }
        }
        EditorGUILayout.EndHorizontal();


    }

    private void DrawPoolItems()
    {
        //상단에 메뉴 2개를 만들자.
        EditorGUILayout.BeginHorizontal();
        {
            GUI.color = new Color(0.19f, 0.76f, 0.08f);
            if(GUILayout.Button("Generate Item"))
            {
                GeneratePoolItem();
            }

            GUI.color = new Color(0.81f, 0.13f, 0.18f);
            if(GUILayout.Button("Generate enum file"))
            {
                GenerateEnumFile();
            }
        }
        EditorGUILayout.EndHorizontal();

        GUI.color = Color.white; //원래 색상으로 복귀.

        EditorGUILayout.BeginHorizontal();
        {
            // 왼쪽 풀리스트 출력부분
            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(300f));
            {
                EditorGUILayout.LabelField("Pooling list");
                EditorGUILayout.Space(3f);
                
                
                scrollPositions[UtilType.Pool] = EditorGUILayout.BeginScrollView
                    (scrollPositions[UtilType.Pool], false, true, 
                        GUIStyle.none, GUI.skin.verticalScrollbar, GUIStyle.none);
                {
                    foreach (PoolingItemSO item in _poolTable.datas)
                    {
                        // 현재 그릴 item이 선택아이템과 동일하면 스타일 지정
                        GUIStyle style = selectedItem[UtilType.Pool] == item
                            ? _selectStyle
                            : GUIStyle.none;
                        EditorGUILayout.BeginHorizontal(style, GUILayout.Height(40f));
                        {
                            EditorGUILayout.LabelField(item.enumName, 
                                GUILayout.Height(40f), GUILayout.Width(240f));

                            EditorGUILayout.BeginVertical();
                            {
                                EditorGUILayout.Space(10f);
                                GUI.color = Color.red;
                                if (GUILayout.Button("X", GUILayout.Width(20f)))
                                {
                                    _poolTable.datas.Remove(item);
                                    AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(item));
                                    EditorUtility.SetDirty(_poolTable);
                                    AssetDatabase.SaveAssets();
                                }
                                GUI.color = Color.white;
                            }
                            EditorGUILayout.EndVertical();
                            
                        }
                        EditorGUILayout.EndHorizontal();
                        
                        // 마지막으로 그린 사각형 정보를 알아옴
                        Rect lastRect = GUILayoutUtility.GetLastRect();

                        if (Event.current.type == EventType.MouseDown
                            && lastRect.Contains(Event.current.mousePosition)) 
                        {
                            inspectorScroll = Vector2.zero;
                            selectedItem[UtilType.Pool] = item;
                            Event.current.Use();
                        }
                        
                        // 삭제 확인 break;
                        if (item == null)
                            break;
                    }
                    // end of foreach
                    
                }
                EditorGUILayout.EndScrollView();
                
                
            }
            EditorGUILayout.EndVertical();
            
            // 인스펙터 그리기
            if (selectedItem[UtilType.Pool] != null)
            {
                inspectorScroll = EditorGUILayout.BeginScrollView(inspectorScroll);
                {
                    EditorGUILayout.Space(2f);
                    Editor.CreateCachedEditor(
                        selectedItem[UtilType.Pool], null, ref _cachedEditor);
                        
                    _cachedEditor.OnInspectorGUI();
                }
                EditorGUILayout.EndScrollView();
            }
        }
        EditorGUILayout.EndHorizontal();
    }
    
    private void GeneratePoolItem()
    {
        Guid guid = Guid.NewGuid(); // 고유한 문자열 키 반환
        
        PoolingItemSO item = CreateInstance<PoolingItemSO>(); // 메모리에만 생성
        item.enumName = guid.ToString();
        
        AssetDatabase.CreateAsset(item, $"{_poolDirectory}/Pool_{item.enumName}.asset");
        _poolTable.datas.Add(item);
        
        EditorUtility.SetDirty(_poolTable);
        AssetDatabase.SaveAssets();
    }

    private void GenerateEnumFile()
    {
        StringBuilder codeBuilder = new StringBuilder();

        foreach (PoolingItemSO item in _poolTable.datas)
        {
            codeBuilder.Append(item.enumName);
            codeBuilder.Append(",");
        }
        
        string code = string.Format(CodeFormat.PoolingTypeFormat, codeBuilder.ToString());
        
        string path = $"{Application.dataPath}/01.Scripts/Core/ObjectPool/PoolingType.cs";
        
        File.WriteAllText(path, code);
        AssetDatabase.Refresh();
    }

    
}

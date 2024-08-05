using ObjectPooling;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public enum UtilType
{
    Pool,
    PowerUp, //�켭ó�� ������ ���׷��̵�
    Effect, //�ش� ������ ����Ʈ
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


    #region �� ������ ���̺� ����
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
        UtilityWindow window = GetWindow<UtilityWindow>("��ƿ��Ƽ");
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

    //��ƿ��Ƽ ���� �¾��ϴ� �Լ�.
    private void SetUpUtility()
    {
        _selectTexture = new Texture2D(1, 1); //1�ȼ�¥�� �ؽ��� �׸�
        _selectTexture.SetPixel(0, 0, new Color(0.31f, 0.40f, 0.50f));
        _selectTexture.Apply();

        _selectStyle = new GUIStyle();
        _selectStyle.normal.background = _selectTexture;
        
        _selectTexture.hideFlags = HideFlags.DontSave;

        _toolbarItemNames = Enum.GetNames(typeof(UtilType));
        
        foreach(UtilType type in Enum.GetValues(typeof(UtilType)))
        {
            if(scrollPositions.ContainsKey(type) == false)
                scrollPositions[type] = Vector2.zero;

            if (selectedItem.ContainsKey(type) == false)
                selectedItem[type] = null;
        }

        if(_poolTable == null)
        {
            _poolTable = CreateAssetTable<PoolingTableSO>(_poolDirectory);
        }
        if(_powerUpTable == null)
        {
            _powerUpTable = CreateAssetTable<PowerUpListSO>(_powerUpDirectory);
        }
        if(_effectTable == null)
        {
            _effectTable = CreateAssetTable<PowerUpEffectListSO>(_effectDirectory);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private T CreateAssetTable<T>(string path) where T : ScriptableObject
    {
        T table = AssetDatabase.LoadAssetAtPath<T>($"{path}/table.asset");
        if (table == null)
        {
            table = ScriptableObject.CreateInstance<T>();

            string fileName = AssetDatabase.GenerateUniqueAssetPath($"{path}/table.asset");
            AssetDatabase.CreateAsset(table, fileName);
            Debug.Log($"{typeof(T).Name} Table Created At : {fileName}");
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
        switch(toolbarIndex)
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

    private T GenerateEffectAsset<T>(string path) where T :PowerUpEffectSO
    {
        Guid guid = Guid.NewGuid();
        T newData = ScriptableObject.CreateInstance<T>();
        newData.code = guid.ToString();
        AssetDatabase.CreateAsset(newData, $"{path}/Effect_{guid}.asset");
        _effectTable.list.Add(newData);
        EditorUtility.SetDirty(_effectTable);
        AssetDatabase.SaveAssets();

        return newData;
    }

    private void DrawEffectItems()
    {
        //��������, ��ų���, ��ų���׷��̵�
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


                scrollPositions[UtilType.Effect] = EditorGUILayout.BeginScrollView(
                    scrollPositions[UtilType.Effect],
                    false, true, GUIStyle.none, GUI.skin.verticalScrollbar, GUIStyle.none);
                {
                    foreach (var so in _effectTable.list)
                    {
                        float labelWidth = 220f;
                        GUIStyle style = selectedItem[UtilType.Effect] == so
                                ? _selectStyle : GUIStyle.none;

                        EditorGUILayout.BeginHorizontal(style, GUILayout.Height(40f));
                        {

                            EditorGUILayout.LabelField(
                                $"[{so.type}]", GUILayout.Width(60f), GUILayout.Height(40f));

                            EditorGUILayout.LabelField(
                                $"[{so.code}]", GUILayout.Width(labelWidth), GUILayout.Height(40f));

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
                            //End of delete!
                        }
                        EditorGUILayout.EndHorizontal();

                        if (so == null)
                            break;

                        //���������� �׸� �簢�� ������ �˾ƿ´�.
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
            if (GUILayout.Button("New PowerUp Item"))
            {
                Guid guid = Guid.NewGuid();
                PowerUpSO newData = CreateInstance<PowerUpSO>();
                newData.code = guid.ToString();
                AssetDatabase.CreateAsset(newData, $"{_powerUpDirectory}/PowerUp_{newData.code}.asset");
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
                EditorGUILayout.LabelField("PowerUp List");
                EditorGUILayout.Space(3f);


                scrollPositions[UtilType.PowerUp] = EditorGUILayout.BeginScrollView(
                    scrollPositions[UtilType.PowerUp],
                    false, true, GUIStyle.none, GUI.skin.verticalScrollbar, GUIStyle.none);
                {

                    foreach(var so in _powerUpTable.list)
                    {
                        float labelWidth = so.icon != null ? 200f : 240f;
                        GUIStyle style = selectedItem[UtilType.PowerUp] == so
                                ? _selectStyle : GUIStyle.none;

                        //���� �׸���.
                        EditorGUILayout.BeginHorizontal(style, GUILayout.Height(40f));
                        {
                            if(so.icon != null)
                            {
                                //������ �׷��ش�.
                                Texture2D previewTexture = AssetPreview.GetAssetPreview(so.icon);
                                GUILayout.Label(
                                    previewTexture, GUILayout.Width(40f), GUILayout.Height(40f));
                            }

                            EditorGUILayout.LabelField(
                                so.code, GUILayout.Width(labelWidth), GUILayout.Height(40f));

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
                            //End of delete!
                        }
                        EditorGUILayout.EndHorizontal();

                        if(so == null)
                            break;

                        //���������� �׸� �簢�� ������ �˾ƿ´�.
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
        //��ܿ� �޴� 2���� ������.
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

        GUI.color = Color.white; //���� �������� ����.

        EditorGUILayout.BeginHorizontal();
        {

            //���� Ǯ����Ʈ ��ºκ�
            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(300f));
            {
                EditorGUILayout.LabelField("Pooling list");
                EditorGUILayout.Space(3f);


                scrollPositions[UtilType.Pool] = EditorGUILayout.BeginScrollView(
                    scrollPositions[UtilType.Pool], 
                    false, true, GUIStyle.none, GUI.skin.verticalScrollbar, GUIStyle.none);
                {

                    foreach(PoolingItemSO item in _poolTable.datas)
                    {

                        //���� �׸� item�� ���þ����۰� �����ϸ� ��Ÿ������
                        GUIStyle style = selectedItem[UtilType.Pool] == item ?
                                                _selectStyle : GUIStyle.none;

                        EditorGUILayout.BeginHorizontal(style, GUILayout.Height(40f));
                        {
                            EditorGUILayout.LabelField(item.enumName, GUILayout.Height(40f), GUILayout.Width(240f));

                            EditorGUILayout.BeginVertical();
                            {
                                EditorGUILayout.Space(10f);
                                GUI.color = Color.red;
                                if (GUILayout.Button("X", GUILayout.Width(20f)))
                                {
                                    //_poolTable.datas ���⼭ �ش��ϴ� �༮�� �����ؾ���
                                    _poolTable.datas.Remove(item);
                                    //Assetdatabase.DeleteAsset����� �̿��ؼ� ������ SO�� �����ؾ���
                                    AssetDatabase.DeleteAsset( AssetDatabase.GetAssetPath(item));
                                    // _poolTable �����ٰ� �̾߱������ ��
                                    EditorUtility.SetDirty(_poolTable);
                                    // SaveAsset�� ���ؼ� �������ָ� ��.
                                    AssetDatabase.SaveAssets();
                                }
                                GUI.color = Color.white;
                            }
                            EditorGUILayout.EndVertical();
                        }
                        EditorGUILayout.EndHorizontal();

                        //���������� �׸� �簢�� ������ �˾ƿ´�.
                        Rect lastRect = GUILayoutUtility.GetLastRect();

                        if(Event.current.type == EventType.MouseDown 
                            && lastRect.Contains(Event.current.mousePosition))
                        {
                            inspectorScroll = Vector2.zero;
                            selectedItem[UtilType.Pool] = item;
                            Event.current.Use();
                        }

                        //�����Ȱ� Ȯ���ϸ� break�� �ɾ��ָ� ��.
                        if (item == null)
                            break;

                    } 
                    //end of foreach

                }
                EditorGUILayout.EndScrollView();   

            }
            EditorGUILayout.EndVertical();

            //�ν����͸� �׷���� ��.
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
        Guid guid = Guid.NewGuid(); //������ ���ڿ� Ű�� ��ȯ��
        
        PoolingItemSO item = CreateInstance<PoolingItemSO>(); //�̰� �޸𸮿��� �����Ѱž�.
        item.enumName = guid.ToString();

        //������ �������� �����߰� ����Ʈ�� �����߾�.
        AssetDatabase.CreateAsset(item, $"{_poolDirectory}/Pool_{item.enumName}.asset");
        _poolTable.datas.Add(item);

        EditorUtility.SetDirty(_poolTable);  //�� ���̺� ������ �Ͼ���� �˷���� ��
        AssetDatabase.SaveAssets(); //����� �͵��� �ν��ؼ� ������ �Ѵ�.
    }

    private void GenerateEnumFile()
    {
        StringBuilder codeBuilder = new StringBuilder();

        foreach(PoolingItemSO item in _poolTable.datas)
        {
            codeBuilder.Append(item.enumName);
            codeBuilder.Append(",");
        }

        string code = string.Format(CodeFormat.PoolingTypeFormat, codeBuilder.ToString());

        string path = $"{Application.dataPath}/01.Scripts/Core/ObjectPool/PoolingType.cs";

        File.WriteAllText(path, code);
        AssetDatabase.Refresh(); //�ٽ� ������ ����
    }

}

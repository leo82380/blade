using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TypeChar
{
    public bool IsComplete = false;

    private Vector3[] _originPostion;
    private float _currentTime = 0;
    private float _delayTime = 0;
    private float _effectTime = 0;
    private int _startIndex;

    private Color _startColor;
    private Color _endColor;

    public TypeChar(int start, Vector3[] vertices, Color32[] colors, Color startColor, Color endColor, float delayTime, float effectTime = 0.5f, float offset = 15f)
    {
        _originPostion = new Vector3[4]; //4ĭ �Ҵ�

        for (int i = 0; i < 4; ++i)
        {
            Vector3 point = vertices[start + i];
            _originPostion[i] = point; //���� ��ġ Ȯ��

            if (i == 0 || i == 3)
                vertices[start + i] = point + new Vector3(offset, 0, 0); //�������� 10f��ŭ �̵��� ������ ������
            else
                vertices[start + i] = point + new Vector3(offset + 0.25f, 0, 0); //�ణ ������ �������

            colors[start + i].a = 0;
        }

        _delayTime = delayTime;
        _effectTime = effectTime;
        _startIndex = start;
        _startColor = startColor;
        _endColor = endColor;
    }

    public void UpdateChar(Vector3[] vertices, Color32[] colors)
    {
        _currentTime += Time.deltaTime;
        if (_currentTime < _delayTime || IsComplete) return; //������ ���°ų� �Ϸ���¸� ����

        float time = _currentTime - _delayTime; //������� ����ð�
        float percent = time / _effectTime;

        for (int i = 0; i < 4; ++i)
        {
            vertices[_startIndex + i] = Vector3.Lerp(vertices[_startIndex + i], _originPostion[i], percent);

            colors[_startIndex + i] = Color.Lerp(_startColor, _endColor, percent);
        }

        if (percent > 1)
        {
            IsComplete = true;
        }
    }
}

public class RoadRunnerEffect : MonoBehaviour
{
    [SerializeField]
    private float _oneCharacterTime = 0.2f;
    [SerializeField]
    private Color _startColor, _endColor;

    private bool _isType = false;

    private TMP_Text _tmpText;

    private void Awake()
    {
        _tmpText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && _isType == false)
        {
            StartEffect("Hello world! This is GGM!");
        }
    }

    public void StartEffect(string text)
    {
        _tmpText.SetText(text);
        _tmpText.color = _endColor;
        _tmpText.ForceMeshUpdate();

        TypeText();
    }

    private void TypeText()
    {
        _isType = true;

        List<TypeChar> charList = new List<TypeChar>();


        TMP_TextInfo textInfo = _tmpText.textInfo;
        Vector3[] vertices = textInfo.meshInfo[0].vertices;  //��Ƽ������ �Ѱ���� �����Ѵ� �������� ����..
        Color32[] vertexColor = textInfo.meshInfo[0].colors32;

        for (int i = 0; i < textInfo.characterCount; ++i)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            if (charInfo.isVisible == false) continue;
            charList.Add(new TypeChar(charInfo.vertexIndex, vertices, vertexColor, _startColor, _endColor, i * 0.1f, _oneCharacterTime));
        }

        _tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32 | TMP_VertexDataUpdateFlags.Vertices);

        StartCoroutine(TypeCoroutine(vertices, vertexColor, charList));

    }

    private IEnumerator TypeCoroutine(Vector3[] vertices, Color32[] colors, List<TypeChar> list)
    {

        bool complete = false;
        int cnt = 0;

        while (complete == false)
        {
            yield return null;
            foreach (TypeChar c in list)
            {
                c.UpdateChar(vertices, colors);
                complete = c.IsComplete; //�ϳ���  false���� �ȱ�����.
            }
            _tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32 | TMP_VertexDataUpdateFlags.Vertices);

            cnt++;
            if (cnt >= 1000000000)
            {
                Debug.Log("�����ڵ� �ߵ�");
                break;
            }
        }

        _isType = false;
    }
}

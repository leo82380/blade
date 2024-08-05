using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TypeWriter : MonoBehaviour
{
    private TMP_Text _tmpText;
    
    private int _tIndex = 0;
    private bool _isTyping = false;

    [SerializeField] private float _oneCharTime = 0.15f;
    [SerializeField] private Color _startColor, _endColor;

    private void Awake()
    {
        _tmpText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if(!_isTyping)
                StartEffect("Hello This is GGM");
            else
            {
                StopAllCoroutines();
                _isTyping = false;
                _tmpText.maxVisibleCharacters = _tmpText.text.Length;
                _tmpText.ForceMeshUpdate();
            }
        }
        
    }

    private void StartEffect(string text)
    {
        _tmpText.SetText(text);
        _tmpText.ForceMeshUpdate(); //텍스트 데이터를 변경했을 경우 한번 호출
        _tIndex = 0;
        _tmpText.maxVisibleCharacters = 0; //최대로 보이는 캐릭터 
        _isTyping = true;
        StartCoroutine(TypeFullText());
    }

    private IEnumerator TypeFullText()
    {
        TMP_TextInfo textInfo = _tmpText.textInfo;
        _tmpText.color = _endColor;

        for(int i = 0; i < textInfo.characterCount; i++)
        {
            yield return StartCoroutine(TypeOneCharacter(textInfo, i));
        }
        _isTyping = false;
    }
    

    private IEnumerator TypeOneCharacter(TMP_TextInfo textInfo, int index)
    {
        _tmpText.maxVisibleCharacters = index + 1;

        _tmpText.ForceMeshUpdate();

        TMP_CharacterInfo cInfo = textInfo.characterInfo[index];

        if (cInfo.isVisible == false)
        {
            yield return new WaitForSeconds(_oneCharTime);
        }
        else
        {
            Vector3[] vertices = textInfo.meshInfo[0].vertices;
            Color32[] colors = textInfo.meshInfo[0].colors32;

            int v0 = cInfo.vertexIndex;
            int v1 = v0 + 1;
            int v2 = v0 + 2;
            int v3 = v0 + 3;

            Vector3 v1Origin = vertices[v1]; //글자의 좌측 상단
            Vector3 v2Origin = vertices[v2]; //글자의 우측 상단

            float currentTime = 0;
            float percent = 0;
            while(percent < 1)
            {
                currentTime += Time.deltaTime;
                percent = currentTime / _oneCharTime;

                float yDelta = Mathf.Lerp(2f, 0, percent);
                vertices[v1] = v1Origin + new Vector3(0, yDelta, 0);
                vertices[v2] = v2Origin + new Vector3(0, yDelta, 0);

                for(int i = 0; i < 4; i++)
                {
                    colors[v0 + i] = Color.Lerp(_startColor, _endColor, percent);
                }

                _tmpText.UpdateVertexData(
                    TMP_VertexDataUpdateFlags.Vertices | TMP_VertexDataUpdateFlags.Colors32);
                yield return null;
            }
        }
        
    }
}

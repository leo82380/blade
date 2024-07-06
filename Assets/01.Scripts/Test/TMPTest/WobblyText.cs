using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WobblyText : MonoBehaviour
{
    private TMP_Text _tmpText; // UGUI, GameObject의 부모

    private void Awake()
    {
        _tmpText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        // 입력된 텍스트 스트링으로 메시정보를 만들어주는 거
        _tmpText.ForceMeshUpdate();
        
        TMP_TextInfo textInfo = _tmpText.textInfo; // 해당 매시에 들어가있는 텍스트 정보

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            
            if (charInfo.isVisible == false)
                continue;
            
            // charInfo.materialReferenceIndex => 0;
            Vector3[] vertices = textInfo.meshInfo[0].vertices;

            int v0 = charInfo.vertexIndex;
            for (int j = 0; j < 4; j++)
            {
                vertices[v0 + j] += new Vector3(0, Mathf.Sin(Time.time * 2f + i) * 0.1f, 0);
            }
        }
        
        _tmpText.UpdateVertexData();
        // var meshInfo = textInfo.meshInfo[0];
        // meshInfo.mesh.vertices = meshInfo.vertices; // 드래프트 데이터를 워킹데이터
        //
        // _tmpText.UpdateGeometry(meshInfo.mesh, 0);
    }
}

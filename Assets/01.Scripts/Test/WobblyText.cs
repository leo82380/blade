using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WobblyText : MonoBehaviour
{
    private TMP_Text _tmpText; //�̳༮�� Ugui , Gameobject�� �θ� 

    private void Awake()
    {
        _tmpText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        //�Էµ� �ؽ�Ʈ ��Ʈ������ �޽������� ������ִ°�  
        _tmpText.ForceMeshUpdate();

        TMP_TextInfo textInfo = _tmpText.textInfo; //�ش� �޽ÿ� ���ִ� �ؽ�Ʈ ����

        for(int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

            if (charInfo.isVisible == false)
                continue;

            //charInfo.materialReferenceIndex => 0
            Vector3[] vertices = textInfo.meshInfo[0].vertices;

            int v0 = charInfo.vertexIndex;
            for(int j = 0; j < 4; j++)
            {
                Vector3 origin = vertices[v0 + j];
                vertices[v0 + j] = origin
                    + new Vector3(0, Mathf.Sin(Time.time * 2f + origin.x), 0);
            }
        }

        _tmpText.UpdateVertexData();
        //var meshInfo = textInfo.meshInfo[0];
        //meshInfo.mesh.vertices = meshInfo.vertices;//�巡��Ʈ �����͸� ��ŷ������

        //_tmpText.UpdateGeometry(meshInfo.mesh, 0);
    }
}

using System;
using System.Collections.Generic;
using DG.Tweening;
using ObjectPooling;
using TMPro;
using UnityEngine;

public enum TextType
{
    Normal,
    Critical,
    Message
}

[Serializable]
public struct TextInfo
{
    public TextType type;
    [ColorUsage(true, true)]
    public Color textColor;
    public float fontSize;
}

public class PopUpText : PoolableMono
{
    [SerializeField] private List<TextInfo> _textInfoList;
    private TextMeshPro _popUpText;

    private void Awake()
    {
        _popUpText = GetComponent<TextMeshPro>();
    }

    public void StartPopUp(string text, Vector3 pos, TextType type, float yDelta = 2f)
    {
        var textInfo = GetTextInfo(type);
        
        _popUpText.SetText(text);
        _popUpText.color = textInfo.textColor;
        _popUpText.fontSize = textInfo.fontSize;
        transform.position = pos;

        float scaleTime = 0.2f;
        float fadeTime = 1.5f;
        
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(2.5f, scaleTime));
        sequence.Append(transform.DOScale(1.2f, scaleTime));
        sequence.Append(transform.DOScale(0.3f, fadeTime));
        sequence.Join(_popUpText.DOFade(0, fadeTime));
        sequence.Join(transform.DOLocalMoveY(pos.y + yDelta, fadeTime));
        sequence.AppendCallback(() => PoolManager.Instance.Push(this));
    }
    
    private TextInfo GetTextInfo(TextType type)
    {
        return _textInfoList.Find(x => x.type == type);
    }

    private void LateUpdate()
    {
        Transform mainCamTrm = Camera.main.transform;
        Vector3 camDirection = (transform.position - mainCamTrm.position).normalized;
        
        transform.rotation = Quaternion.LookRotation(camDirection);
    }

    public override void ResetItem()
    {
        transform.localScale = Vector3.one;
        _popUpText.alpha = 1f;
    }
}

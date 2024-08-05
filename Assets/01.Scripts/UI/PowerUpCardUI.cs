using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpCardUI : MonoBehaviour
{
    [field: SerializeField] public PowerUpSO PowerUp { get; private set; }
    [SerializeField] private TextMeshProUGUI _titleText, _descText;
    [SerializeField] private Image _iconImage;
    [SerializeField] private Button _selectBtn;

    private void OnValidate()
    {
        if(PowerUp != null)
        {
            UpdateUI();
        }
    }

    private void Awake()
    {
        _selectBtn.onClick.AddListener(SelectPowerUp);
    }

    public void SetCardData(PowerUpSO data)
    {
        PowerUp = data;
        UpdateUI();
    }

    private void SelectPowerUp()
    {
        PowerUp.effectList.ForEach(effect => effect.UseEffect());
        UIManager.Instance.Close(WindowEnum.LevelUp);
    }

    private void UpdateUI()
    {
        if (_titleText != null)
            _titleText.text = PowerUp.title;

        if (_descText != null)
            _descText.text = PowerUp.description;

        if (_iconImage != null)
            _iconImage.sprite = PowerUp.icon;
    }
}

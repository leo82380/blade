using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudPanel : MonoBehaviour
{
    [SerializeField] private Image _healthBar, _expBar;
    [SerializeField] private TextMeshProUGUI _coinText;

    private Player _player;
    private void Start()
    {
        PlayerManager.Instance.OnExpChanged += HandleExpChanged;
        PlayerManager.Instance.OnCoinChanged += HandleCoinChanged;
        _player = PlayerManager.Instance.Player;
        _player.HealthCompo.OnHitEvent.AddListener(HandleHealthChanged);
        
        
        HandleCoinChanged(0);
        HandleExpChanged(0);
    }

    private void HandleHealthChanged()
    {
        float fillAmount = _player.HealthCompo.GetNormalizedHealth();
        float duration = 0.2f;
        _healthBar.DOFillAmount(fillAmount, duration);
    }

    private void HandleCoinChanged(int coin)
    {
        Vector3 endScale = Vector3.one * 1.2f;
        float duration = 0.1f;
        _coinText.text = coin.ToString();
        Sequence seq = DOTween.Sequence();
        seq.Append(_coinText.transform.DOScale(endScale, duration));
        seq.Append(_coinText.transform.DOScale(Vector3.one, duration));
    }

    private void HandleExpChanged(int value)
    {
        int maxExp = 200;
        float endValue = (float)value / maxExp;
        float duration = 0.5f;
        _expBar.DOFillAmount(endValue, duration);
    }
}
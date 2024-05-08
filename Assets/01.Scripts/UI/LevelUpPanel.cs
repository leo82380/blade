using DG.Tweening;
using UnityEngine;

public class LevelUpPanel : MonoBehaviour, IWindowPanel
{
    [SerializeField] private float _bottomYPos = -540;
    [SerializeField] private float _topYPos = 540;

    [SerializeField] private RectTransform _rectTrm;

    private bool _isTween = false;

    public void Open()
    {
        if (_isTween) return;
        
        Time.timeScale = 0;
        _isTween = true;
        PlayerManager.Instance.Player.PlayerInput.SetPlayerInput(false);
        
        _rectTrm.DOAnchorPosY(_bottomYPos, 0.8f)
            .SetEase(Ease.OutBounce)
            .SetUpdate(true)
            .OnComplete(() => _isTween = false);
    }

    public void Close()
    {
        if (_isTween) return;
        
        Time.timeScale = 1;
        _isTween = true;
        PlayerManager.Instance.Player.PlayerInput.SetPlayerInput(true);

        _rectTrm.DOAnchorPosY(_topYPos, 0.8f)
            .SetEase(Ease.OutSine)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                PlayerManager.Instance.Player.PlayerInput.SetPlayerInput(true);
                _isTween = false;
                Time.timeScale = 1;
            });
    }
}

using System.Linq;
using DG.Tweening;
using UnityEngine;

public class LevelUpPanel : MonoBehaviour, IWindowPanel
{
    [SerializeField] private float _bottomYPos = -540;
    [SerializeField] private float _topYPos = 540;

    [SerializeField] private RectTransform _rectTrm;

    [SerializeField] private PowerUpCardUI[] _cards;
    [SerializeField] private PowerUpListSO _powerUpList;

    private bool _isTween = false;

    public void Open()
    {
        if (_isTween) return;
        
        Time.timeScale = 0;
        SetUpPowerUpCards();
        _isTween = true;
        PlayerManager.Instance.Player.PlayerInput.SetPlayerInput(false);
        
        _rectTrm.DOAnchorPosY(_bottomYPos, 0.8f)
            .SetEase(Ease.OutBounce)
            .SetUpdate(true)
            .OnComplete(() => _isTween = false);
    }

    private void SetUpPowerUpCards()
    {
        PowerUpSO[] arr = _powerUpList.list.Where(x => x.CheckCanUpgrade()).ToArray();

        if (arr.Length < 3)
        {
            Debug.LogError("Error! must have 3 item at least");
        }

        for (int i = 0; i < 3; i++)
        {
            int index = Random.Range(0, arr.Length - i);
            
            _cards[i].SetCardData(arr[index]);
            arr[index] = arr[arr.Length - 1 - i];
        }
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

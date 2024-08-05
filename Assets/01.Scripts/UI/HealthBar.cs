using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Agent _owner;
    [SerializeField] private Image _fillImage;
    private CanvasGroup _canvasGroup;
    private Transform _mainCamTrm;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        _owner.HealthCompo.OnHitEvent.AddListener(HandleHitEvent);
        _owner.HealthCompo.OnDeadEvent.AddListener(HandleDeadEvent);
        _mainCamTrm = Camera.main.transform;
    }

    private void HandleDeadEvent()
    {
        _canvasGroup.DOFade(0, 1f);
    }

    private void HandleHitEvent()
    {
        float fillAmount = _owner.HealthCompo.GetNormalizedHealth();
        _fillImage.fillAmount = fillAmount;
    }

    private void LateUpdate()
    {
        Vector3 dir = transform.position - _mainCamTrm.position;
        transform.rotation = Quaternion.LookRotation(dir);
    }
}
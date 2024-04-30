using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CircleOrbSkill : Skill
{
    public float orbCooldown = 0.5f;
    public int damage = 5;
    public float knockbackPower = 1f;
    public int initOrbCount = 3;
    public float spinTime = 3f;
    public float spinRadius = 1.7f;

    [SerializeField] private OrbTrm _orbParent;
    [SerializeField] private int _maxOrbCount = 10;
    [SerializeField] private DamageOrb _orbPrefab;

    [HideInInspector] public int currentOrbCount = 0;
    [HideInInspector] public float currentSpinTime = 0;

    private List<Vector3> _deltaPositionList;
    private List<DamageOrb> _orbList;

    private bool _skillUsing = false;
    private bool _orbReduction = false;

    protected override void Start()
    {
        base.Start();
        
        _orbList = new List<DamageOrb>();
        _deltaPositionList = new List<Vector3>();

        for (int i = 0; i < initOrbCount; i++)
        {
            UpgradeAddOrb(false);
        }
        CalculateOffset();
    }

    public bool CanUpgradeAddOrb()
    {
        return currentOrbCount < _maxOrbCount;
    }

    private void CalculateOffset()
    {
        float angle = (360f / currentOrbCount) * Mathf.Deg2Rad;

        for (int i = 0; i < currentOrbCount; i++)
        {
            float currentAngle = angle * i;
            Vector3 pos = new Vector3(Mathf.Cos(currentAngle), 0, Mathf.Sin(currentAngle)) * spinRadius;
            _deltaPositionList[i] = pos;
        }
    }

    public void UpgradeAddOrb(bool reCalculate = true)
    {
        if (currentOrbCount >= _maxOrbCount) return;
        
        DamageOrb orb = Instantiate(_orbPrefab, _orbParent.transform);
        orb.InitializeOrb(this);
        orb.gameObject.SetActive(false);
        _orbList.Add(orb);
        _deltaPositionList.Add(Vector3.zero);
        currentOrbCount++;
        
        if (reCalculate)
            CalculateOffset();
    }

    public override bool UseSkill()
    {
        if (_skillUsing) return false;
        if (base.UseSkill() == false) return false;
        
        currentSpinTime = 0;
        _orbReduction = false;
        OrbExpansion();
        return true;
    }

    protected override void Update()
    {
        base.Update();
        if (_skillUsing == false) return;
        
        currentSpinTime += Time.deltaTime;
        if (currentSpinTime >= spinTime && !_orbReduction)
        {
            _orbReduction = true;
            OrbReduction();
        }
    }

    private void OrbExpansion()
    {
        _skillUsing = true;
        float duration = 0.5f;
        float size = 1f;
        _orbParent.SetFollow(true);

        Sequence seq = DOTween.Sequence();

        for(int i = 0; i < currentOrbCount; i++)
        {
            DamageOrb orb = _orbList[i];
            Vector3 pos = _deltaPositionList[i];

            orb.transform.localPosition = Vector3.zero;
            orb.transform.localScale = Vector3.one * 0.1f;
            orb.gameObject.SetActive(true);

            seq.Join(orb.transform.DOLocalMove(pos, duration));
            seq.Join(orb.transform.DOScale(Vector3.one * size, duration));
        }

        seq.OnComplete(() => StartSkill());
    }

    private void StartSkill()
    {
        _orbParent.SetRotate(true, 360f);
    }

    private void OrbReduction()
    {
        // 0.4초간 리덕션
        // 리덕션 후 스킬 종료
        float duration = 0.4f;
        _orbParent.SetRotate(false, 0);
        
        Sequence seq = DOTween.Sequence();
        for (int i = 0; i < currentOrbCount; i++)
        {
            DamageOrb orb = _orbList[i];
            seq.Join(orb.transform.DOLocalMove(Vector3.zero, duration));
            seq.Join(orb.transform.DOScale(Vector3.one * 0.1f, 0.4f));
        }
        
        seq.OnComplete(() => EndSkill());
    }

    private void EndSkill()
    {
        _orbParent.SetFollow(false);
        _skillUsing = false;
        
        for (int i = 0; i < currentOrbCount; i++)
        {
            _orbList[i].gameObject.SetActive(false);
        }
    }
}

using System.Collections.Generic;
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
        if (base.UseSkill() == false) return false;
        
        OrbExpansion();
        return true;
    }

    private void OrbExpansion()
    {
        _orbParent.SetFollow(true);
        StartSkill();
    }

    private void StartSkill()
    {
        _orbParent.SetRotate(true, 360f);
    }

    private void OrbReduction()
    {
        
    }
}

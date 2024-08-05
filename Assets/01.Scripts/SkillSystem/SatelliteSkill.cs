using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatelliteSkill : Skill
{
    public int damage;
    public float knockBackPower = 0.5f;
    public float checkRadius = 12f;
    public float moveSpeed;
    public float activeCooldown = 6f;
    public int maxCount = 4;
    public float fireCooldown = 1f;


    private int _currentCount = 0;

    private float _lastActiveTime;

    [SerializeField]
    private Vector3[] _deltas =
    {
        new Vector3(1, 1, 0.5f),
        new Vector3(-1, 1, 0.5f),
        new Vector3(1, 1, -0.5f),
        new Vector3(-1, 1, -0.5f),
    };

    [SerializeField] private Satellite _prefab;

    private List<Satellite> _list;
    private bool _isActive = false;

    protected override void Start()
    {
        base.Start();
        _list = new List<Satellite>();
        UpgradeAddSatellite(); //처음 시작할 때 한개는 만들어놓고 시작하기
    }

    public bool CanUpgradeAddSatellite()
    {
        return _currentCount < maxCount;
    }

    public void UpgradeAddSatellite()
    {
        if (_currentCount >= maxCount) return;

        Satellite satellite = Instantiate(_prefab, transform);
        _list.Add(satellite);

        satellite.Initialize(this, _deltas[_currentCount]);
        _currentCount++;
    }

    public override bool UseSkill()
    {
        if (_isActive) return false; //위성이 이미 활성화되어 있으면 취소
        if (base.UseSkill() == false) return false;

        _isActive = true;
        _lastActiveTime = Time.time;

        foreach(Satellite satellite in _list)
        {
            satellite.ShowProcess();
        }

        return true;
    }

    protected override void Update()
    {
        base.Update();

        if(_isActive && _lastActiveTime + activeCooldown <= Time.time)
        {
            _isActive = false;
            foreach(Satellite satellite in _list)
            {
                satellite.HideProcess();
            }
        }
    }
}

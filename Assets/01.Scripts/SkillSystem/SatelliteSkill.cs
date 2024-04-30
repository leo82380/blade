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

    [SerializeField] private Vector3[] _deltas =
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
        UpgradeAddSatellite();
    }
    
    public bool CanUpgradeAddSatellite()
    {
        return _currentCount < maxCount;
    }

    private void UpgradeAddSatellite()
    {
        if (_currentCount >= maxCount) return;
        
        Satellite satellite = Instantiate(_prefab, transform);
        _list.Add(satellite);
        
        satellite.Initialize(this, _deltas[_currentCount]);
        _currentCount++;
    }
}

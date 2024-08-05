using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ThunderStrikeSkill : Skill
{
    public int maxHitEnemyCount = 3;
    public int damage = 5;
    public float hitRadius = 1.5f;
    public int maxCount = 4;
    public float detectRadius = 7f;
    public float thunderInterval = 0.25f;
    
    public List<Thunder> _list;

    [SerializeField] private Thunder _prefab;

    private int _currentCount = 0;
    private Collider[] _colliders;

    protected override void Start()
    {
        base.Start();
        _colliders = new Collider[maxCount]; //벼락의 최대갯수만 적을 감지하면 되니까
        _list = new List<Thunder>();
        UpgradeThunderCount();
    }
    
    public bool CanUpgradeThunderCount()
    {
        return _currentCount < maxCount;
    }

    public void UpgradeThunderCount()
    {
        if (_currentCount >= maxCount) return;

        Thunder thunder = Instantiate(_prefab, transform);
        thunder.Initialize(this);
        _list.Add(thunder);
        _currentCount++;
    }

    public override bool UseSkill()
    {
        if (_cooldownTimer > 0 || skillEnabled == false) return false;

        int count = CheckEnemy();
        if (count >= 1)
        {
            _cooldownTimer = _cooldown;
            StartCoroutine(DelayStrikeCoroutine(count));
            return true;
        }

        return false;
    }

    private IEnumerator DelayStrikeCoroutine(int count)
    {
        var ws = new WaitForSeconds(thunderInterval);

        for(int i = 0; i < _currentCount; i++)
        {
            int enemyIndex = Random.Range(0, count);
            Vector3 position = _colliders[enemyIndex].transform.position;
            _list[i].Strike(position);
            yield return ws;
        }
    }

    private int CheckEnemy()
    {
        return Physics.OverlapSphereNonAlloc(
            player.transform.position, detectRadius, _colliders, whatIsEnemy);
    }
}

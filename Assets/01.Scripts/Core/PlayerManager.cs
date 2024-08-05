using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ModStat
{
    public StatType type;
    public int value;
}

public class PlayerManager : MonoSingleton<PlayerManager>
{
    private readonly int _requiredEXP = 200;
    private int _currentEXP = 0;
    private int _level = 1;

    private Transform _playerTrm = null;
    private int _currentCoin = 0;

    public event Action<int> OnExpChanged;
    public event Action<int> OnLevelChanged;
    public event Action<int> OnCoinChanged;

    public List<ModStat> levelUpStats;

    public Transform PlayerTrm
    {
        get
        {
            if (_playerTrm == null)
            {
                _playerTrm = GameObject.FindGameObjectWithTag("Player").transform;
                if (_playerTrm == null)
                    Debug.LogError("Player does not exists but still try access it");
            }

            return _playerTrm;
        }
    }

    private Player _player;
    public Player Player
    {
        get {
            if(_player == null)
            {
                _player = PlayerTrm.GetComponent<Player>();
            }
            return _player;
        }
    }

    public void AddExp(int value)
    {
        _currentEXP += value;

        if(_currentEXP >= _requiredEXP)
        {
            _level += 1;
            _currentEXP -= _requiredEXP;
            LevelUpProcess();
            OnLevelChanged?.Invoke(_level);
        }
        OnExpChanged?.Invoke(_currentEXP);
    }

    public void AddCoin(int value)
    {
        _currentCoin += value;
        OnCoinChanged?.Invoke(_currentCoin);
    }

    private void LevelUpProcess()
    {
        AgentStat playerStat = Player.Stat;
        levelUpStats.ForEach(modStat 
            => playerStat.AddModifier(modStat.type, modStat.value));

        UIManager.Instance.Open(WindowEnum.LevelUp);
    }
}
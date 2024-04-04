using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    private Transform _playerTrm;
    
    public Transform PlayerTrm
    {
        get
        {
            if (_playerTrm == null)
            {
                _playerTrm = GameObject.FindGameObjectWithTag("Player").transform;
                if (_playerTrm == null)
                {
                    Debug.LogError("Player dose not exist but still try access it.");
                }
            }

            return _playerTrm;
        }
    }
}

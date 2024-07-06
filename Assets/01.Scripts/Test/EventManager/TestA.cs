using System;
using UnityEngine;

public class TestA : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            var evt = Events.ResourceAddEvent;
            evt.amount = 20;
            evt.message = "리소스 20 추가";
            
            EventManager.BroadCast(evt);
        }
    }
}
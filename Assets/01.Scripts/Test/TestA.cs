using UnityEngine;

public class TestA : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            var evt = Events.ResourceAddEvent;
            evt.amount = 20;
            evt.message = "���ҽ� 20 �߰�";

            EventManager.Broadcast(evt);
        }
    }
}

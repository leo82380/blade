using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SInput : MonoBehaviour
{
    public event Action<Vector3> OnMovementEvent;
    public event Action OnFireEvent;

    private void Update()
    {
        CheckMovementInput();
        CheckFireInput();
    }

    private void CheckFireInput()
    {
        if (Input.GetMouseButtonDown(0))
            OnFireEvent?.Invoke();
    }

    private void CheckMovementInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(x, 0, z);
        OnMovementEvent?.Invoke(movement.normalized);
    }
}

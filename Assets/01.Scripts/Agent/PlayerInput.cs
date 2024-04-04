using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public event Action<Vector3> MovementEvent;
    public event Action RollingEvent; 
    public event Action AttackEvent;
    public Vector3 MousePosition { get; private set; }
    public Vector3 KeyInput { get; private set; }
    private bool _playerInputEnabled = true;
    
    public void SetPlayerInput(bool enable)
    {
        _playerInputEnabled = enable;
    }

    private void Update()
    {
        if (_playerInputEnabled == false) return;
        
        CheckMoveInput();
        CheckMouseInput();
        CheckRollingInput();
    }

    private void CheckRollingInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            RollingEvent?.Invoke();
        }
    }

    private void CheckMouseInput()
    {
        MousePosition = Input.mousePosition;
        
        if (Input.GetMouseButtonDown(0))
        {
            AttackEvent?.Invoke();
        }
    }

    private void CheckMoveInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        KeyInput = new Vector3(horizontal, 0, vertical);
        MovementEvent?.Invoke(KeyInput.normalized);
    }
}

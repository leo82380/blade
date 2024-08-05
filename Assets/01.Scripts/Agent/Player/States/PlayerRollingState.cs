using System;
using UnityEngine;

public class PlayerRollingState : PlayerState
{
    public PlayerRollingState(Player player, PlayerStateMachine stateMachine, string boolName) : base(player, stateMachine, boolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Vector3 movingDirection = GetMovingDirection();
        _player.transform.forward = movingDirection;

        _player.DirectMoveCompo.SetMovement(
                        movingDirection * _player.dashSpeed);
    }

    private Vector3 GetMovingDirection()
    {
        Vector3 direction = Vector3.zero;
        switch (_player.rollingDirection)
        {
            case RollingDirection.Mouse:
                direction = CameraManager.Instance.GetTowardMouseDirection(_player.transform, _player.PlayerInput.MousePosition);
                break;
            case RollingDirection.Front:
                Vector3 lastInput = _player.PlayerInput.KeyInput;
                if (lastInput.magnitude > 0)
                {
                    direction = Quaternion.Euler(0, -45f, 0) * lastInput.normalized;
                }
                else
                {
                    direction = _player.transform.forward;
                }
                break;
        }
        return direction;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (_endTriggerCalled)
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
    }

}

using UnityEngine;

public class PlayerRunState : PlayerGroundState
{
    private Vector3 _movementDirection;
    
    public PlayerRunState(Player player, PlayerStateMachine stateMachine, string boolName) : base(player, stateMachine, boolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.PlayerInput.MovementEvent += HandleMovementEvent;
        _player.PlayerVFXCompo.UpdateFootStep(true);
    }
    
    public override void Exit()
    {
        _player.PlayerInput.MovementEvent -= HandleMovementEvent;
        _player.PlayerVFXCompo.UpdateFootStep(false);
        base.Exit();
    }

    private void HandleMovementEvent(Vector3 movement)
    {
        float inputThreshold = 0.05f;
        if (movement.sqrMagnitude < inputThreshold)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
        else
        {
            _movementDirection = movement.normalized;
        }
    }

    public override void UpdateState()
    {
        base.UpdateState();
        Vector3 velocity = Quaternion.Euler(0, -45f, 0) * _movementDirection;
        _player.DirectMoveCompo.SetMovement(velocity * _player.moveSpeed);
    }

    
}

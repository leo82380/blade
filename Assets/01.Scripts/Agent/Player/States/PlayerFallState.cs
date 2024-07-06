using UnityEngine;

public class PlayerFallState : PlayerState
{
    public PlayerFallState(Player player, PlayerStateMachine stateMachine, string boolName) : base(player, stateMachine, boolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.PlayerInput.MovementEvent += HandleMovementEvent;
    }
    
    public override void Exit()
    {
        _player.PlayerInput.MovementEvent -= HandleMovementEvent;
        base.Exit();
    }

    private void HandleMovementEvent(Vector3 movement)
    {
        Vector3 velocity = Quaternion.Euler(0, -45f, 0) * movement;
        _player.DirectMoveCompo.SetMovement(velocity * _player.moveSpeed * 0.5f);
    }

    

    public override void UpdateState()
    {
        base.UpdateState();
        if (_player.MovementCompo.IsGround)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    
}

using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, string boolName) : base(player, stateMachine, boolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.PlayerInput.MovementEvent += HandleMovementEvent;
        _player.MovementCompo.StopImmediately();
    }
    
    public override void Exit()
    {
        _player.PlayerInput.MovementEvent -= HandleMovementEvent;
        base.Exit();
    }

    private void HandleMovementEvent(Vector3 movement)
    {
        float inputThreshold = 0.05f;
        if (movement.sqrMagnitude > inputThreshold)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Run);
        }
    }
}

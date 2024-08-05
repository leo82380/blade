using System;

public abstract class PlayerGroundState : PlayerState
{
    public PlayerGroundState(Player player, PlayerStateMachine stateMachine, string boolName) : base(player, stateMachine, boolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.PlayerInput.AttackEvent += HandleAttackEvent;
        _player.PlayerInput.RollingEvent += HandleRollingEvent;
    }

    public override void Exit()
    {
        _player.PlayerInput.AttackEvent -= HandleAttackEvent;
        _player.PlayerInput.RollingEvent -= HandleRollingEvent;
        base.Exit();
    }

    private void HandleRollingEvent()
    {
        if(_player.MovementCompo.IsGround
            && SkillManager.Instance.GetSkill<RollingSkill>().UseSkill())
        {    
            _stateMachine.ChangeState(PlayerStateEnum.Rolling);   
        }
    }

    private void HandleAttackEvent()
    {
        if (_player.MovementCompo.IsGround)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Attack);
        }
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (_player.MovementCompo.IsGround == false)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Fall);
        }
    }
}

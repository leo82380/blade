using UnityEngine;

public class PlayerHurtState : PlayerState
{
    public PlayerHurtState(Player player, PlayerStateMachine stateMachine, string boolName) : base(player, stateMachine, boolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.PlayerInput.RollingEvent += HandleRollingEvent;
        
        ActionData actionData = _player.HealthCompo.actionData;

        Vector3 dir = actionData.hitNormal;
        dir.y = 0;
        var agentMovement =  _player.MovementCompo as AgentMovement;
        agentMovement.SetMovement(dir * -actionData.knockbackPower, false);
    }

    private void HandleRollingEvent()
    {
        if (SkillManager.Instance.GetSkill<RollingSkill>().UseSkill())
        {
            _stateMachine.ChangeState(PlayerStateEnum.Rolling);
        }
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (_endTriggerCalled)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    public override void Exit()
    {
        _player.PlayerInput.RollingEvent -= HandleRollingEvent;
        base.Exit();
    }
}
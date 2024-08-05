using UnityEngine;

public class PlayerHurtState : PlayerState
{
    public PlayerHurtState(Player player, PlayerStateMachine stateMachine, string boolName) : base(player, stateMachine, boolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        ActionData actionData = _player.HealthCompo.actionData;

        Vector3 dir = actionData.hitNormal;
        dir.y = 0;
        var agentMovement =  _player.MovementCompo as AgentMovement;
        agentMovement.SetMovement(dir * -actionData.knockbackPower, false);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (_endTriggerCalled)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }
}
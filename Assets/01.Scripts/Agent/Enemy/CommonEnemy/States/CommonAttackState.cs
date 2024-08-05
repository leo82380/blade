using UnityEngine;

public class CommonAttackState : EnemyState<CommonStateEnum>
{
    private EnemyMovement movementCompo;
    public CommonAttackState(Enemy enemyBase, EnemyStateMachine<CommonStateEnum> stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        movementCompo = _enemyBase.MovementCompo as EnemyMovement;
    }

    public override void Enter()
    {
        base.Enter();
        //정지하고
        _enemyBase.MovementCompo.StopImmediately();
        //플레이어 위치를 바라보도록 회전하고
        movementCompo.LookToTarget(_enemyBase.targetTrm.position);
    }

    public override void Exit()
    {
        _enemyBase.lastAttackTime = Time.time;
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (_endTriggerCalled)
            _stateMachine.ChangeState(CommonStateEnum.Battle);
    }
}

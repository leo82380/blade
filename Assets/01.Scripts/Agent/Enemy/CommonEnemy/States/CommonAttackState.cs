using System.Collections;
using System.Collections.Generic;
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
        // 정지
        // 플레이어 위치 바라보기
        _enemyBase.MovementCompo.StopImmediately();
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
        // 종료라면
        // 다시 배틀상태
        if (_endTriggerCalled)
        {
            _stateMachine.ChangeState(CommonStateEnum.Battle);
        }
    }
}

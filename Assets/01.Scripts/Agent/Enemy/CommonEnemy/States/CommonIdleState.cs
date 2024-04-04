using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonIdleState : EnemyState<CommonStateEnum>
{
    public CommonIdleState(Enemy enemyBase, EnemyStateMachine<CommonStateEnum> stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
    }

    public override void UpdateState()
    {
        base.UpdateState();
        Collider target = _enemyBase.IsPlayerDetected();
        if (target == null) return; // 플레이어 발견하지 못함
        
        Vector3 direction = target.transform.position - _enemyBase.transform.position;
        direction.y = 0;
        // 플레이어 발건 && 장애물 없음
        if (_enemyBase.IsObstacleDetected(direction.magnitude, direction.normalized) == false)
        {
            _enemyBase.targetTrm = target.transform;
            _stateMachine.ChangeState(CommonStateEnum.Battle); // 전투상태
        }
    }
}

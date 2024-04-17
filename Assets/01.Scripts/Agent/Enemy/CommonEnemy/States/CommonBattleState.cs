using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonBattleState : EnemyState<CommonStateEnum>
{
    protected readonly int _velocityHash = Animator.StringToHash("Velocity");
    private EnemyMovement movementCompo;
    public CommonBattleState(Enemy enemyBase, EnemyStateMachine<CommonStateEnum> stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        movementCompo = _enemyBase.MovementCompo as EnemyMovement;
    }

    private Vector3 _targetDestination; //목적지

    public override void UpdateState()
    {
        base.UpdateState();

        //
        if (movementCompo.NavAgent.enabled)
        {
            _targetDestination = movementCompo.NavAgent.destination;
        }
        
        float distance = (_targetDestination - _enemyBase.transform.position).magnitude;
        if (distance > 0.5f)
        {
            SetDestination(_enemyBase.targetTrm.position);
        }
        
        
        float targetDistance = (_enemyBase.targetTrm.position - _enemyBase.transform.position).magnitude;
        
        bool playerInRange = targetDistance <= _enemyBase.attackDistance;
        bool cooldownPass = _enemyBase.lastAttackTime + _enemyBase.attackCooldown <= Time.time;
        if (playerInRange && cooldownPass)
        {
            _stateMachine.ChangeState(CommonStateEnum.Attack);
        }
        else if (playerInRange)
        {
            _enemyBase.MovementCompo.StopImmediately();
            movementCompo.LookToTarget(_enemyBase.targetTrm.position);
            
        }
        
        float velocity = _enemyBase.MovementCompo.Velocity.magnitude;
        _enemyBase.AnimatorCompo.SetFloat(_velocityHash, velocity);
    }

    public override void Enter()
    {
        base.Enter();
        SetDestination(_enemyBase.targetTrm.position);
    }
    
    // 목적지 설정
    private void SetDestination(Vector3 position)
    {
        _targetDestination = position;
        _enemyBase.MovementCompo.SetDestination(position);
    }
}

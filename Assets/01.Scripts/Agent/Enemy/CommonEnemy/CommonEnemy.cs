using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public enum CommonStateEnum
{
    Idle,
    Battle,
    Attack,
    Dead
}
public class CommonEnemy : Enemy
{
    public EnemyStateMachine<CommonStateEnum> StateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        StateMachine = new EnemyStateMachine<CommonStateEnum>();
        
        // 상태 불러오는 코드
        foreach (CommonStateEnum stateEnum in Enum.GetValues(typeof(CommonStateEnum)))
        {
            string typeName = stateEnum.ToString();
            Type t = Type.GetType($"Common{typeName}State");

            try
            {
                EnemyState<CommonStateEnum> state = Activator.CreateInstance(
                    t, this, StateMachine, typeName) as EnemyState<CommonStateEnum>;
                StateMachine.AddState(stateEnum, state);
            }
            catch (Exception e)
            {
                Debug.LogError($"Enemy Hammer : No State Found [ {typeName} ]");
                Debug.LogError(e.Message);
            }

        }
    }
    
    private void Start()
    {
        StateMachine.Initialize(CommonStateEnum.Idle, this);
    }
    
    private void Update()
    {
        StateMachine.CurrentState.UpdateState();
    }
    
    public override void Attack()
    {
        //공격 로직
    }


    public override void AnimationEndTrigger()
    {
        StateMachine.CurrentState.AnimationTrigger();
    }
    public override void SetDead()
    {
        StateMachine.ChangeState(CommonStateEnum.Dead, true);
        isDead = true;
        CanStateChangeable = false;
    }
}

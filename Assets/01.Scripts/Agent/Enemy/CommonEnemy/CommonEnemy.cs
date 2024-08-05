using System;
using UnityEngine;

public enum CommonStateEnum
{
    Idle,
    Battle,  //ÃßÀû »óÅÂ
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

        //¿©±â¿¡ »óÅÂ¸¦ ºÒ·¯¿À´Â ÄÚµå°¡ ÇÊ¿äÇÏ´Ù.
        foreach (CommonStateEnum stateEnum in Enum.GetValues(typeof(CommonStateEnum)))
        {
            string typeName = stateEnum.ToString();
            Type t = Type.GetType($"Common{typeName}State");

            try
            {
                EnemyState<CommonStateEnum> state =
                    Activator.CreateInstance(t, this, StateMachine, typeName) as EnemyState<CommonStateEnum>;
                StateMachine.AddState(stateEnum, state);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Enemy Hammer : no State found [ {typeName} ] - {ex.Message}");
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
        DamageCasterCompo.CastDamage();
    }

    public override void AnimationEndTrigger()
    {
        StateMachine.CurrentState.AnimationFinishTrigger();
    }

    public override void SetDead()
    {
        StateMachine.ChangeState(CommonStateEnum.Dead, true);
        isDead = true;
        CanStateChangeable = false;
    }
}

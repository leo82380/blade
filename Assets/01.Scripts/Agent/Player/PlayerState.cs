using UnityEngine;

public abstract class PlayerState
{
    protected PlayerStateMachine _stateMachine;
    protected Player _player;

    protected int _animBoolHash;

    protected bool _endTriggerCalled; //애니메이션 종료이벤트 받을 때 필요

    public PlayerState(Player player, PlayerStateMachine stateMachine, string boolName)
    {
        _player = player;
        _stateMachine = stateMachine;
        _animBoolHash = Animator.StringToHash(boolName);
    }

    //들어올때 할일
    public virtual void Enter()
    {
        _player.AnimatorCompo.SetBool(_animBoolHash, true);
        _endTriggerCalled = false;
    }
    //있을 때 할일
    public virtual void UpdateState()
    {
        //지금은 할 게 없고. 나중에 상속받은 애들이 만들거다.
    }
    //나갈때 할일
    public virtual void Exit()
    {
        _player.AnimatorCompo.SetBool(_animBoolHash, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        _endTriggerCalled = true;
    }
}

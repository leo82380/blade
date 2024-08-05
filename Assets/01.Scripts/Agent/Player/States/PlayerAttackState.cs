using System;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    private int _comboCounter = 0;
    private float _lastAttackTime;
    private float _comboWindow = 0.4f; // 키를 누른이후 다시 키를 누르기까지 대기시간
    private readonly int _comboCounterHash = Animator.StringToHash("ComboCounter");

    private Coroutine _delayCoroutine = null;

    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, string boolName) : base(player, stateMachine, boolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _player.PlayerInput.RollingEvent += HandleRollingEvent;

        bool comboCounterOver = _comboCounter > 2;
        bool comboWindowExhaust = Time.time >= _lastAttackTime + _comboWindow;
        if(comboCounterOver || comboWindowExhaust)
        {
            _comboCounter = 0;
        }
        _player.currentComboCounter = _comboCounter;
        _player.AnimatorCompo.speed = _player.attackSpeed;
        _player.AnimatorCompo.SetInteger(_comboCounterHash, _comboCounter);

        Vector3 playerDirection = CameraManager.Instance.GetTowardMouseDirection(
                    _player.transform, _player.PlayerInput.MousePosition);

        //_player.transform.rotation = Quaternion.LookRotation(playerDirection);
        _player.transform.forward = playerDirection;

        float movePower = _player.attackMovement[_comboCounter];
        Vector3 movement = playerDirection * movePower;
        _player.DirectMoveCompo.SetMovement(movement);

        float delayTime = 0.2f;

        _delayCoroutine = _player.StartDelayCallback(delayTime, () =>
        {
            _player.MovementCompo.StopImmediately();
        });
    }


    public override void Exit()
    {
        ++_comboCounter;
        _lastAttackTime = Time.time;
        _player.AnimatorCompo.speed = 1f;

        if(_delayCoroutine != null)
        {
            _player.StopCoroutine(_delayCoroutine);
        }

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

    public override void UpdateState()
    {
        base.UpdateState();
        if(_endTriggerCalled)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    public override void AnimationFinishTrigger()
    {
        _endTriggerCalled = true;
    }
}

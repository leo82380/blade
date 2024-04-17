using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonDeadState : EnemyState<CommonStateEnum>
{
    private bool _isDissolve = false;
    public CommonDeadState(Enemy enemyBase, EnemyStateMachine<CommonStateEnum> stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (_endTriggerCalled && _isDissolve == false)
        {
            _isDissolve = true;
            _enemyBase.StartCoroutine(StartDissolveCoroutine());
        }
    }

    private IEnumerator StartDissolveCoroutine()
    {
        yield return new WaitForSeconds(1f);
        GameObject.Destroy(_enemyBase.gameObject);
    }
}

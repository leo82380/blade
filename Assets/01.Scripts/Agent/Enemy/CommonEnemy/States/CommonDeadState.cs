using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonDeadState : EnemyState<CommonStateEnum>
{
    private bool _isDissolve = false;
    private readonly int _dissolveID = Shader.PropertyToID("_DissolveHeight");
    
    private int _deadbodyLayer = LayerMask.NameToLayer("DeadBody");
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

    public override void Enter()
    {
        base.Enter();
        _enemyBase.gameObject.layer = _deadbodyLayer;
    }

    private IEnumerator StartDissolveCoroutine()
    {
        float curTime = 0;
        float totalTime = 2f;
        Material mat = _enemyBase.meshRenderer.material;
        while (curTime / totalTime <= 1)
        {
            float value = Mathf.Lerp(2, -2, curTime / totalTime);
            mat.SetFloat(_dissolveID, value);
            curTime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);
        GameObject.Destroy(_enemyBase.gameObject);
    }
}

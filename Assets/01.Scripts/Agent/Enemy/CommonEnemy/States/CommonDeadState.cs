using System.Collections;
using System.Collections.Generic;
using ObjectPooling;
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

        int gold = _enemyBase.DropTable.GetDropGold();
        int exp = _enemyBase.DropTable.dropExp;
        
        PlayerManager.Instance.AddExp(exp);

        Vector3 dropDirection = _enemyBase.HealthCompo.actionData.hitNormal * -1;

        for (int i = 0; i < gold; i++)
        {
            Item coin = PoolManager.Instance.Pop(PoolingType.Item_Coin) as Item;
            Vector3 realDir = Quaternion.Euler(0, Random.Range(-30f, 30f), 0) * dropDirection;
            coin.SetItemData(_enemyBase.transform.position, realDir);
        }


        // 코인이 튀는 방향은 ActionData의 노말의 역벡터로 잡아주고
        // 코인의 갯수만큼 for문을 생성하여 던저주면 됨
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

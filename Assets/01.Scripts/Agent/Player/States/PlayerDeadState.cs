using System.Collections;
using UnityEngine;

public class PlayerDeadState : PlayerState
{
    private bool _isDissolve = false;
    public PlayerDeadState(Player player, PlayerStateMachine stateMachine, string boolName) : base(player, stateMachine, boolName)
    {
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (_endTriggerCalled && _isDissolve == false)
        {
            _isDissolve = true;
            _player.StartCoroutine(StartDissolveCoroutine());
        }
    }

    private IEnumerator StartDissolveCoroutine()
    {
        yield return new WaitForSeconds(1f);
        GameObject.Destroy(_player.gameObject);
    }
}

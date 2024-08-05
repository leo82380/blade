using UnityEngine;
using UnityEngine.Events;

public class EnemyAnimationTrigger : MonoBehaviour
{
    [SerializeField] Enemy _enemy;

    public UnityEvent OnAnimationEvent;

    private void AnimationEnd()
    {
        _enemy.AnimationEndTrigger();
    }

    private void AnimationEvent()
    {
        OnAnimationEvent?.Invoke();
    }
    
    private void DamageCastEvent()
    {
        _enemy.Attack();
    }
}

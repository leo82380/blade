using UnityEngine;

public abstract class Enemy : Agent
{
    [Header("Common settings")] 
    public float moveSpeed;
    public float battleTime;
    public bool isActive;
    
    protected float _defaultMoveSpeed;

    [SerializeField] protected LayerMask _whatIsPlayer;
    [SerializeField] protected LayerMask _whatIsObstacle;

    [Header("Attack Settings")] 
    public float runAwayDistance;
    public float attackDistance;
    public float attackCooldown;
    [SerializeField] protected int _maxCheckEnemy = 1;
    [HideInInspector] public float lastAttackTime;
    [HideInInspector] public Transform targetTrm;
    protected Collider[] _enemyCheckColliders;
    
    protected override void Awake()
    {
        base.Awake();
        _defaultMoveSpeed = moveSpeed;
        _enemyCheckColliders = new Collider[_maxCheckEnemy];
    }
    
    public virtual Collider IsPlayerDetected()
    {
        int cnt = Physics.OverlapSphereNonAlloc(transform.position, runAwayDistance, _enemyCheckColliders, _whatIsPlayer);
        
        return cnt >= 1 ? _enemyCheckColliders[0] : null;
    }
    
    public virtual bool IsObstacleDetected(float distance, Vector3 direction)
    {
        return Physics.Raycast(transform.position, direction, distance, _whatIsObstacle);
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, runAwayDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
        Gizmos.color = Color.white;
    }

    public abstract void AnimationEndTrigger();
}

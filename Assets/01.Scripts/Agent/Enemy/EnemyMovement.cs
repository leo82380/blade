using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour, IMovement
{
    public Vector3 Velocity => _navAgent.velocity;
    public bool IsGround { get; }

    private Enemy _enemy;
    private NavMeshAgent _navAgent;
    public void Initialize(Agent agent)
    {
        _enemy = agent as Enemy;
        _navAgent = GetComponent<NavMeshAgent>();
        _navAgent.speed = _enemy.moveSpeed;
    }
    
    public void SetDestination(Vector3 destination)
    {
        _navAgent.isStopped = false;
        _navAgent.SetDestination(destination);
    }

    public void StopImmediately()
    {
        _navAgent.isStopped = true;
    }

    public void LookToTarget(Vector3 targetPos)
    {
        Vector3 toward = targetPos - transform.position;
        toward.y = 0;
        transform.rotation = Quaternion.LookRotation(toward);
    }
    
    public void SetMovement(Vector3 movement, bool isRotation = true)
    {
        // 여기선 안씀
    }

    
}

using UnityEngine;

public class AgentMovement : MonoBehaviour, IMovement ,IDirectMoveable
{
    [SerializeField] private float gravity = -9.8f;

    protected CharacterController _characterController;
    private Agent _agent;
    private Quaternion _targetRotation;

    #region 속도 관련 로직
    private Vector3 _velocity;
    
    public Vector3 Velocity => _velocity;
    private float _verticalVelocity;
    private Vector3 _movementInput;
    #endregion
    
    public bool IsGround => _characterController.isGrounded;

    public void Initialize(Agent agent)
    {
        _characterController = GetComponent<CharacterController>();
        _agent = agent;
    }

    private void FixedUpdate()
    {
        //중력 계산
        ApplyGravity();
        ApplyRotation();
        //이동
        Move();
    }

    private void ApplyRotation()
    {
        float rotateSpeed = 8f;
        transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation, 
            Time.fixedDeltaTime * rotateSpeed);
    }

    private void ApplyGravity()
    {
        if (IsGround && _verticalVelocity < 0)
        {
            _verticalVelocity = -0.03f;
        }
        else
        {
            
            _verticalVelocity += gravity * Time.fixedDeltaTime;
        }
        _velocity.y = _verticalVelocity;
    }

    private void Move()
    {
        _characterController.Move(_velocity);
    }

    public void SetMovement(Vector3 movement, bool isRotation = true)
    {
        _velocity = movement * Time.fixedDeltaTime;

        if (_velocity.sqrMagnitude > 0 && isRotation == true)
        {
            _targetRotation = Quaternion.LookRotation(_velocity);
        } 
    }
    
    public void StopImmediately()
    {
        _velocity = Vector3.zero;
    }

    // public void SetDestination(Vector3 destination)
    // {
    //     // 플레이어는 안씀 (NavMesh기반)
    // }

    public void GetKnockback(Vector3 force)
    {
        // 현재는 넉백 구현 안함
    }
}

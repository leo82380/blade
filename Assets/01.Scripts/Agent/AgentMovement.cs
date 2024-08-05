using UnityEngine;

public class AgentMovement : MonoBehaviour, IMovement, IDirectMovable
{
    [SerializeField] private float _gravity = -9.8f;

    protected CharacterController _characterController;
    private Agent _agent;
    private Quaternion _targetRotation;

    #region ¼Óµµ °ü·Ã ·ÎÁ÷
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
        //Áß·Â °è»ê
        ApplyGravity();
        ApplyRotation();
        //ÀÌµ¿
        Move();
    }

    private void ApplyRotation()
    {
        float rotateSpeed = 8f;
        transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation, Time.fixedDeltaTime * rotateSpeed);
    }

    private void ApplyGravity()
    {
        if (IsGround && _verticalVelocity < 0)
        {
            _verticalVelocity = -0.03f;
        }
        else
        {
            _verticalVelocity += _gravity * Time.fixedDeltaTime;
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

    //public void SetDestination(Vector3 destination)
    //{
    //    //ÇÃ·¹ÀÌ¾î´Â ÀÌ ÇÔ¼ö¸¦ ¾È¾´´Ù. (NavMesh±â¹Ý)
    //}

    public void GetKnockback(Vector3 force)
    {
        _agent.HealthCompo.actionData.knockbackPower = force.magnitude;
        Vector3 dir = force;
        dir.y = 0;
        _targetRotation = Quaternion.LookRotation(-dir.normalized);
        _agent.transform.rotation = _targetRotation;
        (_agent as Player).StateMachine.ChangeState(PlayerStateEnum.Hurt);  
    }
}

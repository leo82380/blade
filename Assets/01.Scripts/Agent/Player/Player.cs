using System;
using UnityEngine;

public enum RollingDirection
{
    Mouse,
    Front
}

public enum PlayerStateEnum
{
    Idle,
    Run,
    Fall,
    Attack,
    Rolling
}

public class Player : Agent
{
    [Header("Setting Values")]
    public float moveSpeed = 8f;
    public float dashSpeed = 20f;
    public RollingDirection rollingDirection = RollingDirection.Mouse;

    [Header("Attack Settings")] 
    public float attackSpeed = 1f;
    public int currentComboCounter = 0;
    public float[] attackMovement;
    
    public PlayerStateMachine StateMachine { get; protected set; }
    [SerializeField] private PlayerInput _playerInput;
    public float fallSpeed;
    public PlayerInput PlayerInput => _playerInput;
    public PlayerVFX PlayerVFXCompo => VFXCompo as PlayerVFX;

    protected override void Awake()
    {
        base.Awake();

        StateMachine = new PlayerStateMachine();

        foreach (PlayerStateEnum stateEnum in Enum.GetValues(typeof(PlayerStateEnum)))
        {
            string typeName = stateEnum.ToString();

            try
            {
                Type t = Type.GetType($"Player{typeName}State");
                PlayerState state = Activator.CreateInstance(
                    t, this, StateMachine, typeName) as PlayerState;
                StateMachine.AddState(stateEnum, state);
            }
            catch (Exception e)
            {
                Debug.LogError($"{typeName} is loading error! check Message");
                Debug.LogError(e.Message);
            }

        }
        
        // StateMachine.AddState(
        //     PlayerStateEnum.Idle, new PlayerIdleState(this, StateMachine, "Idle"));
        // StateMachine.AddState(
        //     PlayerStateEnum.Run, new PlayerRunState(this, StateMachine, "Run"));
        // StateMachine.AddState(
        //     PlayerStateEnum.Fall, new PlayerFallState(this, StateMachine, "Fall"));
    }

    protected void Start()
    {
        StateMachine.Initialize(PlayerStateEnum.Idle, this);
    }

    protected void Update()
    {
        StateMachine.CurrentState.UpdateState();
    }

    public override void Attack()
    {
        
    }

    public void PlayBladeVFX()
    {
        PlayerVFXCompo.PlayBladeVFX(currentComboCounter);
    }
}

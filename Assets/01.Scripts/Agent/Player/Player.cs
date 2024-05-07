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
    Rolling,
    Dead
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
    }

    protected void Start()
    {
        StateMachine.Initialize(PlayerStateEnum.Idle, this);
    }

    protected void Update()
    {
        StateMachine.CurrentState.UpdateState();
        
        if (Input.GetKeyDown(KeyCode.O))
        {
            SkillManager.Instance.GetSkill<CircleOrbSkill>().UnlockSkill();
            SkillManager.Instance.GetSkill<SatelliteSkill>().UnlockSkill();
            SkillManager.Instance.GetSkill<ThunderStrikeSkill>().UnlockSkill();
            SkillManager.Instance.GetSkill<BombSkill>().UnlockSkill();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            //SkillManager.Instance.GetSkill<ThunderStrikeSkill>().UpgradeThunderCount();
            SkillManager.Instance.GetSkill<BombSkill>().UseSkill();
        }
    }

    public override void Attack()
    {
        bool success = DamageCasterCompo.CastDamage();

        if (success && currentComboCounter == 2)
        {
            SkillManager.Instance.GetSkill<ThunderStrikeSkill>().UseSkill();
        }
    }


    public void PlayBladeVFX()
    {
        PlayerVFXCompo.PlayBladeVFX(currentComboCounter);
    }
    
    public override void SetDead()
    {
        // 지금은 아무것도 안함
        StateMachine.ChangeState(PlayerStateEnum.Dead);
    }
    
}

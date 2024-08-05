using UnityEngine;

public delegate void CooldownInfo(float current, float total);

public class Skill : MonoBehaviour
{
    public bool skillEnabled = false;
    [SerializeField] protected float _cooldown; //ÀÌ ½ºÅ³ÀÇ ÄðÅ¸ÀÓ
    [SerializeField] protected bool _isAutoSkill;

    [HideInInspector] public Player player;
    public event CooldownInfo CooldownEvent;

    protected float _cooldownTimer;
    public LayerMask whatIsEnemy;

    public void UnlockSkill()
    {
        if (skillEnabled) return;
        skillEnabled = true;
        if(_isAutoSkill)
        {
            SkillManager.Instance.AddEnableSkill(this);
        }
    }

    protected virtual void Start()
    {
        player = PlayerManager.Instance.Player;
    }

    protected virtual void Update()
    {
        if (_cooldownTimer > 0)
        {
            _cooldownTimer -= Time.deltaTime;
            if (_cooldownTimer <= 0)
            {
                _cooldownTimer = 0;
            }
            CooldownEvent?.Invoke(_cooldownTimer, _cooldown);
        }
    }

    public virtual bool UseSkill()
    {
        if (_cooldownTimer > 0 || skillEnabled == false) return false;

        _cooldownTimer = _cooldown;
        return true;
    }
}

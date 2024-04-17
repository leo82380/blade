using UnityEngine;

public class DamageCaster : MonoBehaviour
{
    [SerializeField][Range(0.5f, 3f)]
    private float _casterRadius = 1f;
    [SerializeField][Range(0, 1f)]
    private float _casterInterpolation = 0.5f;
    [SerializeField] [Range(0, 3f)] 
    private float _castingRange = 1f;
    
    public LayerMask targetLayer;
    private Agent _owner;
    
    public void InitCaster(Agent agent)
    {
        _owner = agent;
    }

    public bool CastDamage()
    {
        Vector3 startPos = GetStartPos();
        bool isHit = Physics.SphereCast
            (startPos, _casterRadius, transform.forward, out RaycastHit hit, _castingRange, targetLayer);

        if (isHit)
        {
            Debug.Log($"타격 : {hit.collider.name}");
            if (hit.collider.TryGetComponent(out IDamageable damageable))
            {
                // 나중에 스탯에서 가져옴
                int damage = _owner.Stat.GetDamage();
                float knockbackPower = 3f;
                damageable.ApplyDamage(damage, hit.point, hit.normal, knockbackPower, _owner);
            }
        }
        
        return isHit;
    }

    private Vector3 GetStartPos()
    {
        return transform.position + transform.forward * -_casterInterpolation * 2;
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(GetStartPos(), _casterRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GetStartPos() + transform.forward * _castingRange, _casterRadius);
        Gizmos.color = Color.white;
    }
#endif
}

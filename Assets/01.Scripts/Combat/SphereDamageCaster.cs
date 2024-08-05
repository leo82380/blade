using UnityEngine;

public class SphereDamageCaster : DamageCaster
{
    [SerializeField]
    [Range(0.5f, 3f)]
    private float _casterRadius = 1f;
    [SerializeField]
    [Range(0, 1f)]
    private float _casterInterpolation = 0.5f;
    [SerializeField]
    [Range(0, 3f)]
    private float _castingRange = 1f;
    
    public override bool CastDamage()
    {
        Vector3 startPos = GetStartPos();
        bool isHit = Physics.SphereCast(
            startPos, 
            _casterRadius, 
            transform.forward, 
            out RaycastHit hit, 
            _castingRange, targetLayer);

        if(isHit)
        {
            //Debug.Log($"Ÿ�� : {hit.collider.name}");
            if(hit.collider.TryGetComponent<IDamageable>(out IDamageable health))
            {
                int damage = _owner.Stat.GetDamage(); //������ ������
                float knockbackPower = 3f; //���߿� �������κ��� �����;� ��.

                health.ApplyDamage(damage, hit.point, hit.normal, knockbackPower, _owner, DamageType.Melee);
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
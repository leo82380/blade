using UnityEngine;

public class ActionData
{
    public Vector3 hitPoint;
    public Vector3 hitNormal;
    public bool isCritical; //이전 피격이 크리티컬이였니?
    public DamageType lastDamageType;
}

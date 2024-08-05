using UnityEngine;

public class ActionData
{
    public Vector3 hitPoint;
    public Vector3 hitNormal;
    public bool isCritical; //ÀÌÀü ÇÇ°ÝÀÌ Å©¸®Æ¼ÄÃÀÌ¿´´Ï?
    public DamageType lastDamageType;
    public float knockbackPower;
}

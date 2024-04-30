using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatelliteSkill : Skill
{
    public int damage;
    public float knockBackPower = 0.5f;
    public float checkRadius = 12f;
    public float moveSpeed;
    public float activeCooldown = 6f;
    public int maxCount = 4;
    public float fireCooldown = 1f;
}

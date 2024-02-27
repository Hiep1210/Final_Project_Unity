using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorStat : ScriptableObject
{
    [Header("Stat - Component: ")]
    public int hp;
    public float moveSpeed;
    public float damage;
    public float bulletAttackRate;
    public float swordAttackRate;

    public float invincibleTime;
    public float knockBackTime;
    public float forceKnockBack;
}

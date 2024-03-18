using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorStat : ScriptableObject
{
    [Header("Stat - Component: ")]
    public int hp;
    public float moveSpeed;
    public int damage;

    public float invincibleTime;
    public float knockBackTime;
    public float forceKnockBack;
}

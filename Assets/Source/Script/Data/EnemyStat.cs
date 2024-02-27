using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStat",menuName ="ScriptableObject/EnemyStat")]
public class EnemyStat : ActorStat
{
    [Header("Enemy-Stat")]
    public float movingSpeed;
    public float chassingSpeed;
    public float damage;
}

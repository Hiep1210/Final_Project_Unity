using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStat",menuName ="ScriptableObject/EnemyStat")]
public class EnemyStat : ActorStat
{
    [Header("Enemy-Stat")]
    public float chassingSpeed;
    public float moveSpeedAttack;
    public float timeChangeStateFirst;
    public float rateTimeAttack;
}

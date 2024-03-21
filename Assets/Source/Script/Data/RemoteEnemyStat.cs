using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RemoteEnemyStat", menuName = "ScriptableObject/Remote Enemy Stat")]
public class RemoteEnemyStat : ActorStat
{
    public float ChassingSpeed;
    public float AttackRate;
    public float StartStateRate;
    public float bulletFlySpeed;
    public int minScore;
    public int maxScore;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CloseEnemyStat", menuName = "ScriptableObject/Close Enemy Stat")]
public class CloseEnemyStat : ActorStat
{
    public float ChassingSpeed;
    public float AttackRate;
    public float StartStateRate;
}

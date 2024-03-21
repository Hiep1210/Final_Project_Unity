using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerStat", menuName = "ScriptableObject/Player Stat")]
public class PlayerStat : ActorStat
{
    public float RollSpeed;
    public float RollRateTime;
}

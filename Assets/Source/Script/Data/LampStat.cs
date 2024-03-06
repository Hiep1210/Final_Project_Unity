using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LampStat", menuName = "ScriptableObject/LampStat")]
public class LampStat : ScriptableObject
{
    [Header("Lamp Stat: ")]
    public int hp;
}

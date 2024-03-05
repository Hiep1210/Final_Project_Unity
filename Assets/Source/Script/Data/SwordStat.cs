using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SwordStat", menuName = "ScriptableObject/SwordStat")]
public class SwordStat : ScriptableObject
{
    [Header("Sword Stat: ")]
    public int damage;
    public float timeRate;
}

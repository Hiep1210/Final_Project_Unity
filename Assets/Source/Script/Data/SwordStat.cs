using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SwordStat", menuName = "ScriptableObject/SwordStat")]
public class SwordStat : ScriptableObject
{
    [Header("Sword Stat: ")]
    public float damage;
    public float timeRate;
}

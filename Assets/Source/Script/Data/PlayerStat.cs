using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStat", menuName = "ScriptableObject/PlayerStat")]
public class PlayerStat : ActorStat
{
    [Header("Player Stat: ")]
    public float speedScroll;
    public float scrollTimeRate;
}

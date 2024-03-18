using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameTag
{
    Player,
    Enemy,
    Ground
}

public enum Direction
{
    Left,
    Right,
    Up,
    Down
}

public enum PlayerStateAnimator
{
    Idle,
    Walk,
    Scroll,
    GoHit,
    Dead
}

public enum EnemyStateAnimator
{
    Idle,
    Moving,
    Chassing,
    Attack,
    GotHit,
    Dead
}

public enum MethodDetect
{
    OverlapCircle,
    Raycast
}
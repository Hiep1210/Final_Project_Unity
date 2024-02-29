using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    Run,
    Scroll,
    SwordAttack,
    FireBullet,
    Dead
}

public enum EnemyStateAnimator
{
    Idle,
    Moving,
    Chassing,
    GotHit,
    Dead
}

public enum MethodDetect
{
    OverlapCircle,
    Raycast
}
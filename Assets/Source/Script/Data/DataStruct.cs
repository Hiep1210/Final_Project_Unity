using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameTag
{
    Player,
    Enemy,
    Collectable
}

public enum GameLayer
{

}

public enum Direction
{
    Left,
    Right,
    Up,
    Down,
    None
}

public enum StateAnimtorPlayer
{
    Idle,
    Walk,
    Death,
    Roll,
    Scour
}

public enum StateAnimatorEnemy
{
    Idle,
    Walk,
    Chassing,
    Hit,
    Death,
    Fly
}


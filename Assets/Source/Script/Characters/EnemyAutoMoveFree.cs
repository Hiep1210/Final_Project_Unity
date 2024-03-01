using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AutoMoveFree))]
public class EnemyAutoMoveFree : Enemy
{
    private AutoMoveFree m_AutoMoveFree;

    protected override void Awake()
    {
        base.Awake();
        m_AutoMoveFree = GetComponent<AutoMoveFree>();
        m_AutoMoveFree.MoveSpeed = m_enemyStat.moveSpeed;
        m_AutoMoveFree.DistanceMoveAuto = distanceMove;
        InitFSMEnemy(this);
    }

    protected override void MoveChecking()
    {
        base.MoveChecking();
        m_AutoMoveFree.AutoMovingFree();
    }
}

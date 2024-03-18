using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;
using System.Diagnostics;
using System;

[RequireComponent(typeof(DetectPlayer))]
public class Enemy : Actor
{
    protected DetectPlayer m_detectPlayer;
    protected EnemyStat m_enemyStat;

    protected StateMachine<EnemyStateAnimator> m_fsmEnemy;
    protected EnemyStateAnimator m_previewState;

    protected override void Awake()
    {
        base.Awake();
        m_detectPlayer = GetComponent<DetectPlayer>();

        if (actorStat != null)
        {
            m_enemyStat = actorStat as EnemyStat;
        }

        m_currentmoveSpeed = m_enemyStat.moveSpeed;
    }

    protected virtual void Start()
    {

    }

    protected void InitFSMEnemy(MonoBehaviour monoBehaviour)
    {
        m_fsmEnemy = StateMachine<EnemyStateAnimator>.Initialize(monoBehaviour);
        m_fsmEnemy.ChangeState(EnemyStateAnimator.Idle);
        Invoke("ChangeStateForFirstTime", m_enemyStat.timeChangeStateFirst);
    }

    protected void ChangeStateForFirstTime()
    {
        m_fsmEnemy.ChangeState(EnemyStateAnimator.Moving);
    }

    protected void ChangeState(EnemyStateAnimator newStateChange)
    {
        m_previewState = m_fsmEnemy.State;
        m_fsmEnemy.ChangeState(newStateChange);
    }

    protected void InvokeChangeStateCo(EnemyStateAnimator newStateChange, float extraTime = 0f)
    {
        StartCoroutine(ChangeStateCo(newStateChange, extraTime));
    }

    private IEnumerator ChangeStateCo(EnemyStateAnimator newStateChange, float extraTime = 0f)
    {
        AnimationClip animationClip = Helper.GetClip(animator, newStateChange.ToString());
        if (animationClip != null)
        {
            yield return new WaitForSeconds(animationClip.length + extraTime);
        }
        ChangeState(newStateChange);
    }
}

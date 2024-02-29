using MonsterLove.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DetectPlayer))]
public class Enemy : Actor
{
    public float distanceMove;
    
    protected StateMachine<EnemyStateAnimator> m_fsmEnemy;
    protected DetectPlayer m_detectPlayer;
    protected EnemyStat m_enemyStat;
    protected Vector3 m_startPos;

    protected override void Awake()
    {
        base.Awake();
        m_detectPlayer = gameObject.GetComponent<DetectPlayer>();
        if(actorStat != null)
        {
            m_enemyStat = actorStat as EnemyStat;
        }
        m_startPos = transform.position;
    }

    protected void InitFSMEnemy(MonoBehaviour behaviour)
    {
        //FSM_MethodGen.GenAllMethodState<EnemyStateAnimator>();
        m_fsmEnemy = StateMachine<EnemyStateAnimator>.Initialize(behaviour);
        m_fsmEnemy.ChangeState(EnemyStateAnimator.Moving);
    }

    // Update is called once per frame
    void Update()
    {
        MoveChecking();
    }

    protected virtual void MoveChecking()
    {

    }
}

using MonsterLove.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AutoMoveFree))]
[RequireComponent(typeof(DetectTarget))]
public class EnemyClose : Actor
{
    private AutoMoveFree m_autoMoveFree;
    private DetectTarget m_detectTarget;

    private StateMachine<StateAnimatorEnemy> m_enemyFSM;

    private StateAnimatorEnemy m_previewState;

    private CloseEnemyStat m_closeEnemyStat;

    private bool m_isHit;

    private bool m_isAttacked;

    private float m_currTimeAttack;

    private bool m_isStartState;

    private float m_currentTimeState;

    private float m_rateStartState;

    private float m_curSpeedStat;
    private float m_curChassingStat;
    private float m_curAttackRate;

    public float CurMoveSpeedStat { get => m_curSpeedStat; set => m_curSpeedStat = value; }
    public float CurChassingStat { get => m_curChassingStat; set => m_curChassingStat = value; }
    public CloseEnemyStat CloseEnemyStat { get => m_closeEnemyStat; set => m_closeEnemyStat = value; }
    public float CurAttackRate { get => m_curAttackRate; set => m_curAttackRate = value; }

    protected override void Awake()
    {
        base.Awake();

        m_autoMoveFree = GetComponent<AutoMoveFree>();
        m_detectTarget = GetComponent<DetectTarget>();

        if (actorStat != null)
        {
            m_closeEnemyStat = actorStat as CloseEnemyStat;

            m_curSpeedStat = m_closeEnemyStat.MoveSpeed;
            m_curChassingStat = m_closeEnemyStat.ChassingSpeed;
            m_curAttackRate = m_closeEnemyStat.AttackRate;
        }

        //FSM_MethodGen.GenAllMethodState<StateAnimatorEnemy>();
        InitEnemyCloseFSM();

        m_rateStartState = m_closeEnemyStat.StartStateRate;
        m_currentTimeState = m_rateStartState;
        m_isStartState = false;
    }

    private void Update()
    {
        if (!m_isStartState)
        {
            m_currentTimeState -= Time.deltaTime;
            if (m_currentTimeState <= 0)
            {
                m_isStartState = true;
            }
        }

        if (m_isStartState)
        {
            ActiveState();

            if (m_isAttacked)
            {
                ReduceTimeAction(ref m_isAttacked, ref m_currTimeAttack, m_curAttackRate);
            }
        }

    }

    public void FixedUpdate()
    {
        if (m_isStartState || !IsDeadActor)
        {
            MoveChecking();
        }
    }

    private void MoveChecking()
    {
        if (m_autoMoveFree.Player == null) return;

        Vector3 dir = transform.position - m_autoMoveFree.Player.transform.position;
        m_hozDir = dir.normalized.x > 0 ? 1 : -1;
        if (m_hozDir < 0)
        {
            FlipEnemy(Direction.Left);
            m_autoMoveFree.AutoMovingClose();
        }

        if (m_hozDir > 0)
        {
            FlipEnemy(Direction.Right);
            m_autoMoveFree.AutoMovingClose();
        }
    }


    protected void FlipEnemy(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                if (spriteRenderer.transform.localScale.x > 0)
                {
                    spriteRenderer.transform.localScale = new Vector3(-Mathf.Abs(spriteRenderer.transform.localScale.x), spriteRenderer.transform.localScale.y, spriteRenderer.transform.localScale.z);
                }
                break;
            case Direction.Right:
                if (spriteRenderer.transform.localScale.x < 0)
                {
                    spriteRenderer.transform.localScale = new Vector3(Mathf.Abs(spriteRenderer.transform.localScale.x), spriteRenderer.transform.localScale.y, spriteRenderer.transform.localScale.z);
                }
                break;
        }
    }


    protected override void IsDead()
    {
        base.IsDead();

        if (m_currentHp <= 0)
        {
            m_currentHp = 0;

            ChangeState(StateAnimatorEnemy.Death);
        }
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(GameTag.Player.ToString()))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player != null)
            {
                if (!m_isAttacked)
                {
                    player.TakeDamage(this, m_closeEnemyStat.Damage);

                    m_isHit = true;

                    m_isAttacked = true;
                }
            }
        }
    }


    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(GameTag.Player.ToString()))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player != null)
            {
                m_isHit = false;
            }
        }
    }

    #region
    private void InitEnemyCloseFSM()
    {
        //FSM_MethodGen.GenAllMethodState<StateAnimtorPlayer>();

        m_enemyFSM = StateMachine<StateAnimatorEnemy>.Initialize(this);

        m_enemyFSM.ChangeState(StateAnimatorEnemy.Idle);

    }


    private void ChangeState(StateAnimatorEnemy newStateEnemy)
    {
        m_previewState = m_enemyFSM.State;
        m_enemyFSM.ChangeState(newStateEnemy);
    }

    private void ActiveState()
    {
        if (m_isDead) return;

        if (!m_isHit)
        {
            if (m_detectTarget.IsDetectedWalk && !m_detectTarget.IsDetectedChassing)
            {
                ChangeState(StateAnimatorEnemy.Walk);
            }

            if ((m_detectTarget.IsDetectedChassing && m_detectTarget.IsDetectedWalk) || (m_detectTarget.IsDetectedChassing && !m_detectTarget.IsDetectedWalk))
            {
                ChangeState(StateAnimatorEnemy.Chassing);
            }
        }
        else
        {
            ChangeState(StateAnimatorEnemy.Hit);
        }
    }

    void Idle_Enter()
    {
        m_rb.velocity = Vector2.zero;
        m_currentSpeed = m_curSpeedStat;
        m_autoMoveFree.MoveSpeed = m_currentSpeed;
    }
    void Idle_Update()
    {
        Helper.PlayAnim(animator, StateAnimatorEnemy.Idle.ToString());

        if (m_isHit)
        {
            ChangeState(StateAnimatorEnemy.Hit);
        }

    }
    void Idle_Exit()
    {
    }
    void Walk_Enter()
    {
        m_currentSpeed = m_curSpeedStat;
        m_autoMoveFree.MoveSpeed = m_currentSpeed;
    }
    void Walk_Update()
    {
        Helper.PlayAnim(animator, StateAnimatorEnemy.Walk.ToString());
    }
    void Walk_Exit() { }
    void Chassing_Enter()
    {
        m_currentSpeed = m_curChassingStat;
        m_autoMoveFree.MoveSpeed = m_currentSpeed;
    }
    void Chassing_Update()
    {
        Helper.PlayAnim(animator, StateAnimatorEnemy.Chassing.ToString());
    }
    void Chassing_Exit() { }
    void Hit_Enter() { }
    void Hit_Update()
    {
        Helper.PlayAnim(animator, StateAnimatorEnemy.Hit.ToString());
    }
    void Hit_Exit() { }
    void Death_Enter() { }
    void Death_Update()
    {
        Helper.PlayAnim(animator, StateAnimatorEnemy.Death.ToString());

        m_rb.velocity = Vector2.zero;

        if(deadVfx != null)
        {
            GameObject deadVfxClone = GameObject.Instantiate(deadVfx, transform.position, Quaternion.identity);
            Destroy(deadVfxClone, 0.15f);
        }

        gameObject.SetActive(false);
        Destroy(gameObject, 0.15f);
    }
    void Death_Exit() { }

    #endregion
}

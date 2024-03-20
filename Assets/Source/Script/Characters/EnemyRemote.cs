using MonsterLove.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AutoMoveFree))]
[RequireComponent(typeof(DetectTarget))]
public class EnemyRemote : Actor
{
    private AutoMoveFree m_autoMoveFree;

    private DetectTarget m_detectTarget;

    private StateMachine<StateAnimatorEnemy> m_enemyRemoteFSM;

    private StateAnimatorEnemy m_previewState;

    private RemoteEnemyStat m_remoteEnemyStat;

    private bool m_isHit;

    private bool m_isAttacked;

    private float m_currTimeAttack;

    private bool m_isStartState;

    private float m_currentTimeState;

    private float m_rateStartState;

    [Header("Bullet Remote Enemy: ")]
    public GameObject bulletRemoteEnemyPrefabs;

    protected override void Awake()
    {
        base.Awake();

        m_autoMoveFree = GetComponent<AutoMoveFree>();
        m_detectTarget = GetComponent<DetectTarget>();

        if (actorStat != null)
        {
            m_remoteEnemyStat = actorStat as RemoteEnemyStat;
        }

        InitEnemyRemoteFSM();

        m_rateStartState = m_remoteEnemyStat.StartStateRate;
        m_currentTimeState = m_rateStartState;
        m_isStartState = false;
    }

    private void Update()
    {
        Debug.Log("Current Hp Enemy: " + m_currentHp);

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
                ReduceTimeAction(ref m_isAttacked, ref m_currTimeAttack, m_remoteEnemyStat.AttackRate);
            }
        }
    }

    public void FixedUpdate()
    {
        if (m_isStartState && !IsDeadActor)
        {
            MoveChecking();
        }
    }

    private void MoveChecking()
    {


        Vector3 dir = transform.position - m_autoMoveFree.Player.transform.position;

        m_hozDir = dir.normalized.x > 0 ? 1 : -1;
        if (m_hozDir < 0)
        {
            FlipEnemy(Direction.Left);
            m_autoMoveFree.AutoMoveRemote();
        }

        if (m_hozDir > 0)
        {
            FlipEnemy(Direction.Right);
            m_autoMoveFree.AutoMoveRemote();
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

    private void GotHitChecking()
    {
        if (m_autoMoveFree.Player.IsDeadActor) return;

        if (!m_isAttacked)
        {
            if (bulletRemoteEnemyPrefabs != null)
            {
                GameObject bulletGObject = Instantiate(bulletRemoteEnemyPrefabs.gameObject, new Vector3(transform.position.x, transform.position.y - 2, transform.position.z), Quaternion.identity);

                BulletRemoteEnemy bullet = bulletGObject.gameObject.GetComponent<BulletRemoteEnemy>();

                bullet.ActorOwner = this;

                bullet.Player = m_autoMoveFree.Player;

                bullet.FlySpeed = m_remoteEnemyStat.bulletFlySpeed;
            }

            ChangeState(StateAnimatorEnemy.Hit);

            m_isAttacked = true;
            m_isHit = true;
        }
        else
        {
            ReduceTimeAction(ref m_isAttacked, ref m_currTimeAttack, m_remoteEnemyStat.AttackRate);
            m_isHit = false;
        }
    }

    #region FSM - Enemy Remote

    private void ActiveState()
    {
        if (m_isDead) return;

        if (!m_isHit)
        {
            if (m_detectTarget.IsDetectedWalk && !m_detectTarget.IsDetectedChassing)
            {
                ChangeState(StateAnimatorEnemy.Fly);
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


        GotHitChecking();
    }
    private void ChangeState(StateAnimatorEnemy newStateEnemy)
    {
        m_previewState = m_enemyRemoteFSM.State;
        m_enemyRemoteFSM.ChangeState(newStateEnemy);
    }

    private void InitEnemyRemoteFSM()
    {
        //FSM_MethodGen.GenAllMethodState<StateAnimtorPlayer>();

        m_enemyRemoteFSM = StateMachine<StateAnimatorEnemy>.Initialize(this);

        m_enemyRemoteFSM.ChangeState(StateAnimatorEnemy.Chassing);
    }

    void Idle_Enter()
    {

        m_rb.velocity = Vector2.zero;
        m_currentSpeed = m_remoteEnemyStat.MoveSpeed;
        m_autoMoveFree.MoveSpeed = m_currentSpeed;
    }
    void Idle_Update()
    {
        Helper.PlayAnim(animator, StateAnimatorEnemy.Idle.ToString());
    }
    void Idle_Exit() { }
    void Fly_Enter()
    {
        m_currentSpeed = m_remoteEnemyStat.MoveSpeed;
        m_autoMoveFree.MoveSpeed = m_currentSpeed;
    }
    void Fly_Update() { Helper.PlayAnim(animator, StateAnimatorEnemy.Fly.ToString()); }
    void Fly_Exit() { }
    void Chassing_Enter()
    {
        m_currentSpeed = m_remoteEnemyStat.ChassingSpeed;
        m_autoMoveFree.MoveSpeed = m_currentSpeed;
    }
    void Chassing_Update() { Helper.PlayAnim(animator, StateAnimatorEnemy.Chassing.ToString()); }
    void Chassing_Exit() { }
    void Hit_Enter() { }
    void Hit_Update() { Helper.PlayAnim(animator, StateAnimatorEnemy.Hit.ToString()); }
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
            gameObject.SetActive(false);
            Destroy(gameObject, 0.15f);
        }
    }
    void Death_Exit() { }
    #endregion
}


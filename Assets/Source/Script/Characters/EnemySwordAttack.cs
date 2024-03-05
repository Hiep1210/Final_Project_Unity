using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AutoMoving))]
public class EnemySwordAttack : Enemy
{
    private AutoMoving m_autoMoving;
    private Player m_player;
    private bool m_isAttacked;
    private float m_currentAttackTime;
    

    protected override void Awake()
    {
        base.Awake();
        m_autoMoving = GetComponent<AutoMoving>();
        InitFSMEnemy(this);
    }

    protected override void Start()
    {
        base.Start();
        m_player = m_detectPlayer.playerTarget;
        m_autoMoving.TargetPlayer = m_player;
        m_autoMoving.MoveSpeed = m_currentmoveSpeed;
        m_autoMoving.Sp = spriteRenderer;
    }

    private void Update()
    {
        ActiveStateChecking();
        ReduceActionTime(ref m_isAttacked, ref m_currentAttackTime, m_enemyStat.rateTimeAttack);
    }
    public override void TakeDamage(Actor whoHit, int damage)
    {
        if (m_isDead) return;

        base.TakeDamage(whoHit, damage);

        if (!m_isInvincible && m_currentHp > 0)
        {
            ChangeState(EnemyStateAnimator.GotHit);
        }
    }

    public override void Dead()
    {
        base.Dead();

        ChangeState(EnemyStateAnimator.Dead);
    }

    #region FSM - Enemy

    private void ActiveStateChecking()
    {
        if (m_isKnockBack || m_isDead) return;

        if (m_player.IsDead || !m_player.gameObject.activeInHierarchy)
        {
            ChangeState(EnemyStateAnimator.Idle);
        }

        if (!m_detectPlayer.DetectChassing && !m_detectPlayer.DetectAttack && !m_player.IsDead)
        {
            ChangeState(EnemyStateAnimator.Moving);
        }

        if (m_detectPlayer.DetectChassing && !m_detectPlayer.DetectAttack && !m_player.IsDead)
        {
            ChangeState(EnemyStateAnimator.Chassing);
        }

        if (!m_isAttacked)
        {
            if (m_detectPlayer.DetectChassing && m_detectPlayer.DetectAttack && !m_player.IsDead)
            {
                ChangeState(EnemyStateAnimator.Attack);
            }
        }
    }


    void Idle_Enter() { }
    void Idle_Update()
    {
        Helper.PlayAnim(animator, EnemyStateAnimator.Idle.ToString());
        m_rb.velocity = Vector2.zero;
    }
    void Idle_Exit() { }
    void Moving_Enter()
    {
        m_currentmoveSpeed = m_enemyStat.moveSpeed;
        m_autoMoving.MoveSpeed = m_currentmoveSpeed;
    }
    void Moving_Update()
    {
        Helper.PlayAnim(animator, EnemyStateAnimator.Moving.ToString());
        m_autoMoving.Moving();
    }
    void Moving_Exit() { }
    void Chassing_Enter()
    {
        m_currentmoveSpeed = m_enemyStat.chassingSpeed;
        m_autoMoving.MoveSpeed = m_currentmoveSpeed;
    }
    void Chassing_Update()
    {
        Helper.PlayAnim(animator, EnemyStateAnimator.Chassing.ToString());
        m_autoMoving.Moving();
    }
    void Chassing_Exit() { }
    void Attack_Enter()
    {
        m_currentmoveSpeed = m_enemyStat.moveSpeedAttack;
        m_autoMoving.MoveSpeed = m_currentmoveSpeed;

    }
    void Attack_Update()
    {
        m_rb.velocity = Vector2.zero;

        Helper.PlayAnim(animator, EnemyStateAnimator.Attack.ToString());
    }
    void Attack_Exit() { }

    void GotHit_Enter()
    {
        DistanceKnockBack();
    }
    void GotHit_Update()
    {
        Helper.PlayAnim(animator, EnemyStateAnimator.GotHit.ToString());
    }
    void GotHit_Exit()
    {
    }
    void Dead_Enter() 
    {
        m_rb.velocity = Vector2.zero;
    }
    void Dead_Update()
    {
        Helper.PlayAnim(animator, EnemyStateAnimator.Dead.ToString());
    }
    void Dead_Exit()
    {

    }
    #endregion
}

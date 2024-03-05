using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Actor : MonoBehaviour
{
    [Header("Component References: ")]
    public ActorStat actorStat;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public CapsuleCollider2D capSunCollider;

    [Header("Layer - Actor: ")]
    [LayerList]
    public int normalLayer;
    [LayerList]
    public int invincibleLayer;
    [LayerList]
    public int deadLayer;

    [Header("Visual Effect: ")]
    public GameObject knockBackVfx;

    protected Actor m_whoHit;
    protected Rigidbody2D m_rb;
    protected int m_currentHp;
    protected float m_currentmoveSpeed;
    protected bool m_isKnockBack;
    protected bool m_isInvincible;
    protected bool m_isDead;
    protected float m_horizontal;
    protected float m_vertical;

    public bool IsDead { get => m_isDead; set => m_isDead = value; }

    protected virtual void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_rb.gravityScale = 0f;
        m_rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        m_currentHp = actorStat.hp;
    }

    public virtual void TakeDamage(Actor whoHit, int damage)
    {
        if (m_isInvincible || m_isKnockBack) return;

        if (whoHit != null)
        {
            m_whoHit = whoHit;
            m_currentHp -= m_whoHit.actorStat.damage;
            if (m_currentHp <= 0)
            {
                Dead();
            }
            else
            {
                KnockBack();
            }
        }
        else
        {
            Dead();
        }
    }

    public virtual void Dead()
    {

        if (m_rb != null)
        {
            m_rb.velocity = Vector2.zero;
        }

        m_currentHp = 0;

        gameObject.layer = deadLayer;

        m_isDead = true;
    }

    protected void KnockBack()
    {
        if (m_isKnockBack || m_isInvincible || !gameObject.activeInHierarchy) return;

        m_isKnockBack = true;
        m_isInvincible = false;

        StartCoroutine(StopKnockBackTimeCo(actorStat.knockBackTime));
    }

    private IEnumerator StopKnockBackTimeCo(float timeKnockBack)
    {
        yield return new WaitForSeconds(timeKnockBack);

        m_isKnockBack = false;
        m_isInvincible = true;

        gameObject.layer = invincibleLayer;

        StartCoroutine(StopInvincibleTimeCo(actorStat.invincibleTime));
    }

    private IEnumerator StopInvincibleTimeCo(float timeInvincible)
    {
        yield return new WaitForSeconds(timeInvincible);

        m_isInvincible = false;
        gameObject.layer = normalLayer;
    }

    public void DistanceKnockBack()
    {
        if (m_rb != null)
        {
            Vector3 dir = m_whoHit.transform.position - transform.position;
            dir.Normalize();
            Debug.Log(1);
            if (dir.x > 0)
            {
                m_rb.velocity = new Vector2(-actorStat.forceKnockBack, m_rb.velocity.y);
            }
            else if (dir.x < 0)
            {
                m_rb.velocity = new Vector2(actorStat.forceKnockBack, m_rb.velocity.y);
            }
        }
    }

    protected void FlipCharacters(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                if (spriteRenderer.transform.localScale.x > 0)
                {
                    spriteRenderer.transform.localScale = new Vector3(spriteRenderer.transform.localScale.x * -1f, spriteRenderer.transform.localScale.y, spriteRenderer.transform.localScale.z);
                }
                break;
            case Direction.Right:
                if (spriteRenderer.transform.localScale.x < 0)
                {
                    spriteRenderer.transform.localScale = new Vector3(spriteRenderer.transform.localScale.x * -1f, spriteRenderer.transform.localScale.y, spriteRenderer.transform.localScale.z);
                }
                break;
            case Direction.Up:
                if (spriteRenderer.transform.localScale.y < 0)
                {
                    spriteRenderer.transform.localScale = new Vector3(spriteRenderer.transform.localScale.x, spriteRenderer.transform.localScale.y * -1f, spriteRenderer.transform.localScale.z);
                }
                break;
            case Direction.Down:
                if (spriteRenderer.transform.localScale.y > 0)
                {
                    spriteRenderer.transform.localScale = new Vector3(spriteRenderer.transform.localScale.x, spriteRenderer.transform.localScale.y * -1f, spriteRenderer.transform.localScale.z);
                }
                break;
        }
    }

    protected void ReduceActionTime(ref bool isAction, ref float currentTimeAction, float timeRateAction)
    {
        if (!isAction) return;

        currentTimeAction += Time.deltaTime;
        if (currentTimeAction >= timeRateAction)
        {
            currentTimeAction = 0;
            isAction = false;
        }
    }

}

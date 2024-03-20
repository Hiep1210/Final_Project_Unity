using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Actor : MonoBehaviour
{
    [Header("Actor Stat: ")]
    public ActorStat actorStat;
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    [Header("Layer: ")]
    [LayerList]
    public int normalLayer;
    [LayerList]
    public int invincibleLayer;
    [LayerList]
    public int deadLayer;

    public GameObject deadVfx;
    public GameObject flashVfx;

    protected Rigidbody2D m_rb;
    protected int m_hozDir;
    protected int m_vertDir;
    protected float m_currentHp;
    protected float m_currentSpeed;
    protected float m_currentDamage;

    protected Actor m_whoHit;

    protected bool m_isDead;
    protected bool m_isKnockBack;
    protected bool m_isInvincible;

    public bool IsDeadActor { get => m_isDead; set => m_isDead = value; }

    public float CurrentHp { get => m_currentHp; set => m_currentHp = value; }
    public float CurrentDamage { get => m_currentDamage; set => m_currentDamage = value; }
    public float CurrentMoveSpeed { get => m_currentSpeed; set => m_currentSpeed = value; }

    protected virtual void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();

        m_rb.gravityScale = 0f;
        m_rb.freezeRotation = true;

        m_currentHp = actorStat.Hp;
        m_currentSpeed = actorStat.MoveSpeed;
        m_currentDamage = actorStat.Damage;
    }


    public void TakeDamage(object whoHit, float damage)
    {
        if (whoHit != null && m_currentHp > 0)
        {

            m_currentHp -= damage;

            if (m_currentHp <= 0)
            {
                m_currentHp = 0;

                IsDead();
            }
        }
        else if (m_currentHp <= 0)
        {
            m_currentHp = 0;
            IsDead();
        }
    }

    protected virtual void IsDead()
    {
        m_isDead = true;
        gameObject.layer = deadLayer;

        if (m_rb)
        {
            m_rb.velocity = Vector3.zero;
        }
    }

    protected void FlipFollowDirection(Direction direction)
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
        }
    }

    public void ReduceTimeAction(ref bool isAction, ref float currentTimeResetAction, float rateTimeAction)
    {
        if (!isAction) return;

        currentTimeResetAction -= Time.deltaTime;
        if (currentTimeResetAction <= 0)
        {
            isAction = false;
            currentTimeResetAction = rateTimeAction;
        }
    }
}

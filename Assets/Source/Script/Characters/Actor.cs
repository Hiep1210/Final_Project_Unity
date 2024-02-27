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
    public LayerMask normal;
    public LayerMask invincible;
    public LayerMask deadLayer;

    [Header("Visual Effect: ")]
    public GameObject knockBackVfx;

    protected Rigidbody2D m_rb;
    protected int m_currentHp;
    protected float m_currentmoveSpeed;
    protected bool m_isFireBullet;
    private bool isDead;
    protected float m_horizontal;
    protected float m_vertical;

    public bool IsDead { get => isDead; set => isDead = value; }

    protected virtual void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_rb.gravityScale = 0f;
        m_currentHp = actorStat.hp;
        m_currentmoveSpeed = actorStat.moveSpeed;
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
                    spriteRenderer.transform.localScale = new Vector3(spriteRenderer.transform.localScale.x, spriteRenderer.transform.localScale.y * -1f,  spriteRenderer.transform.localScale.z);
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
        if(currentTimeAction >= timeRateAction)
        {
            currentTimeAction = 0;
            isAction = false;
        }
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class AutoMoving : MonoBehaviour
{
    private Player player;
    private float moveSpeed;
    private Vector3 dir;
    private SpriteRenderer sp;
    private Rigidbody2D m_rb;
    private bool m_isMoving;

    public Player TargetPlayer { get => player; set => player = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public Vector3 Dir { get => dir; set => dir = value; }
    public SpriteRenderer Sp { get => sp; set => sp = value; }

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_rb.gravityScale = 0f;
    }

    private void FixedUpdate()
    {
        //Moving();
    }

    public void Moving()
    {
        MovingChecking();

        if (m_isMoving)
        {
            dir = player.transform.position - transform.position;
            dir.Normalize();
            Flip();
            m_rb.velocity = moveSpeed * dir;
        }
    }

    private void Flip()
    { 
        if(dir.x > 0)
        {
            if(sp.transform.localScale.x < 0)
            {
                sp.transform.localScale = new Vector3(sp.transform.localScale.x * -1f, sp.transform.localScale.y, sp.transform.localScale.z);
            }
        }
        else
        {
            if (sp.transform.localScale.x > 0)
            {
                sp.transform.localScale = new Vector3(sp.transform.localScale.x * -1f, sp.transform.localScale.y, sp.transform.localScale.z);
            }
        }
    }

    private void MovingChecking()
    {
        float distance = Vector2.Distance(player.transform.position, transform.position);

        if(distance < 1)
        {
            m_isMoving = false;
        }
        else
        {
            m_isMoving = true;
        }
    }
}

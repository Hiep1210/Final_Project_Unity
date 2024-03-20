using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMoveFree : MonoBehaviour
{
    public bool m_isAutoMove = true;

    private Rigidbody2D m_rb;
    private Player m_player;

    private float m_moveSpeed;
    private Vector3 m_TargetDirection;
    private Vector3 m_destination;
    private bool m_isReached;
    public float m_distanceCheck;

    private bool m_isDesRandom;
    private float m_currDesTimeRandom;
    public float desTimeRandom;
    public int xMinDesRandom;
    public int xMaxDesRandom;
    public int yMinDesRandom;
    public int yMaxDesRandom;

    public float MoveSpeed { get => m_moveSpeed; set => m_moveSpeed = value; }
    public Player Player { get => m_player; }

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();

        m_player = GameObject.FindObjectOfType<Player>();

        m_destination = m_player.transform.position;

    }

    private void Update()
    {
        //AutoMovingClose();
        //AutoMoveRemote();
    }

    public void AutoMovingClose()
    {
        if (m_player == null) return;

        if (m_isAutoMove)
        {
            IsReachedChecking(m_distanceCheck);

            m_destination = m_player.transform.position;

            if (!m_isReached)
            {
                Moving(m_destination);
            }
            else
            {
                m_rb.velocity = Vector3.zero;
            }
        }
    }

    private void Moving(Vector3 destination)
    {
        m_TargetDirection = destination - transform.position;
        Vector3 dir = m_TargetDirection.normalized;
        m_rb.velocity = dir * m_moveSpeed;
    }


    private void IsReachedChecking(float distanceAutoDonMove)
    {
        float distanceMyToTarget = Vector2.Distance(transform.position, m_player.transform.position);

        if (distanceMyToTarget <= distanceAutoDonMove)
        {
            m_isReached = true;
        }
        else
        {
            m_isReached = false;
        }
    }

    public void AutoMoveRemote()
    {
        if (m_player == null) return;

        if (m_isAutoMove)
        {
            IsReachedChecking(m_distanceCheck);

            GetDestinationMaxRandom();

            Moving(m_destination);
        }
    }

    private void GetDestinationMaxRandom()
    {
        if (m_isDesRandom)
        {
            m_currDesTimeRandom -= Time.deltaTime;
            if (m_currDesTimeRandom <= 0f)
            {
                m_currDesTimeRandom = desTimeRandom;
                m_isDesRandom = false;
            }
        }

        if (!m_isDesRandom)
        {
            m_destination = m_player.transform.position +
                           new Vector3(UnityEngine.Random.Range(xMinDesRandom, xMaxDesRandom), UnityEngine.Random.Range(yMinDesRandom, yMaxDesRandom), 0);

            m_isDesRandom = true;
        }
    }



    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawLine(transform.position, m_destination);

        Gizmos.color = Color.red;

        if (m_player != null)
        {
            Gizmos.DrawLine(transform.position, m_player.transform.position);
        }
    }
}
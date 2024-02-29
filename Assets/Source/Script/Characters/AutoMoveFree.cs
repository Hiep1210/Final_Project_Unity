using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMoveFree : MonoBehaviour
{
    [Header("Option: ")]
    public bool isAutoMove = true;
    public bool isRotation = true;

    [Header("Setting: ")]
    private float distanceMoveAuto;
    private float moveSpeed;

    private float m_maxMovePosXRight;
    private float m_minMovePosXLeft;
    private float m_maxMovePosYUp;
    private float m_minMovePosYDown;

    private Vector3 m_startPos;
    private Vector3 m_destinationPosRandom;
    private Vector3 m_moveDir;
    private bool m_isReached;
    private float angleRotation;

    private Rigidbody2D m_rb;

    public float DistanceMoveAuto { get => distanceMoveAuto; set => distanceMoveAuto = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public Vector3 DestinationPosRandom { get => m_destinationPosRandom; set => m_destinationPosRandom = value; }
    public Vector3 MoveDir { get => m_moveDir; set => m_moveDir = value; }

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_rb.isKinematic = true;
        m_startPos = transform.position;
        FAutoMoveFree();
    }

    private void FixedUpdate()
    {
        //AutoMoveFree();
    }

    private void FAutoMoveFree()
    {
        GetDistanceMove();
        GetDestinationPos(ref m_destinationPosRandom);
        GetDirRandom(ref m_moveDir);
    }

    public void AutoMovingFree()
    {
        if (isAutoMove)
        {
            IsDestinated();
            if (m_isReached)
            {
                GetDestinationPos(ref m_destinationPosRandom);
                GetDirRandom(ref m_moveDir);
            }
            RotationMove();
            MoveAuto(m_moveDir, moveSpeed);
        }
    }

    private void IsDestinated()
    {
        bool result = false;
        float distanceCurrentToDestinationPos = Vector2.Distance(transform.position, m_destinationPosRandom);
        if (distanceCurrentToDestinationPos <= 0.1f)
        {
            result = true;
        }
        m_isReached = result;
    }

    private void GetDistanceMove()
    {
        m_minMovePosXLeft = m_startPos.x - distanceMoveAuto;
        m_maxMovePosXRight = m_startPos.x + distanceMoveAuto;
        m_minMovePosYDown = m_startPos.y - distanceMoveAuto;
        m_maxMovePosYUp = m_startPos.y + distanceMoveAuto;
    }

    private void GetDestinationPos(ref Vector3 destinationPosRandom)
    {
        float posXRandom = Random.Range(m_minMovePosXLeft, m_maxMovePosXRight);
        float posYRandom = Random.Range(m_minMovePosYDown, m_maxMovePosYUp);
        destinationPosRandom = new Vector2(posXRandom, posYRandom);
    }

    private void GetDirRandom(ref Vector3 moveDir)
    {
        moveDir = m_destinationPosRandom - transform.position;
        moveDir.Normalize();
    }

    private void MoveAuto(Vector3 moveDir, float moveSpeed)
    {
        m_rb.velocity = moveDir * moveSpeed;
    }

    private void RotationMove()
    {
        if (isRotation)
        {
            angleRotation = Mathf.Atan2(m_moveDir.y, m_moveDir.x) * Mathf.Rad2Deg;

            if (m_moveDir.x > 0)
            {
                angleRotation = Mathf.Clamp(angleRotation, -41, 41);
                transform.rotation = Quaternion.Euler(0, 0, angleRotation);
                Flip(Direction.Right);
            }
            else if (m_moveDir.x < 0)
            {
                float newAngleRotation = angleRotation + 180f;
                newAngleRotation = Mathf.Clamp(newAngleRotation, 25, 325);
                transform.rotation = Quaternion.Euler(0, 0, newAngleRotation);
                Flip(Direction.Left);
            }
        }
    }

    private void Flip(Direction moveDirection)
    {
        switch (moveDirection)
        {
            case Direction.Left:
                if (transform.localScale.x > 0)
                {
                    transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
                }
                break;
            case Direction.Right:
                if (transform.localScale.x < 0)
                {
                    transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
                }
                break;
            case Direction.Up:
                if (transform.localScale.y < 0)
                {
                    transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * -1f, transform.localScale.z);
                }
                break;
            case Direction.Down:
                if (transform.localScale.y < 0)
                {
                    transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * -1f, transform.localScale.z);
                }
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Helper.ChangAlpha(Color.yellow, 0.8f);
        float height = m_maxMovePosYUp - m_minMovePosYDown;
        float width = m_maxMovePosXRight - m_minMovePosXLeft;
        Vector3 sizeCube = new Vector3(width, height, 0f);
        Vector3 centerCubePos = m_startPos;
        Gizmos.DrawWireCube(centerCubePos, sizeCube);
        Gizmos.color = Helper.ChangAlpha(Color.magenta, 0.6f);
        Gizmos.DrawLine(transform.position, m_destinationPosRandom);
    }
}

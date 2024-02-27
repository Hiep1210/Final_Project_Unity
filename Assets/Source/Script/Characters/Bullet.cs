using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public LayerMask layerCheck;

    public float moveSpeed;
    private Vector2 m_bulletDir;
    private Vector3 m_previewPos;

    public Vector2 BulletDir { get => m_bulletDir; set => m_bulletDir = value; }

    private void Awake()
    {
        m_previewPos = transform.position;
    }

    private void Update()
    {
        if (m_bulletDir == Vector2.zero) return;
        transform.Translate(m_bulletDir * moveSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        BulletCheck();
    }

    private void BulletCheck()
    {
        Vector3 dir = transform.position - m_previewPos;
        float distance = dir.magnitude;
        dir.Normalize();
        RaycastHit2D raycast = Physics2D.Raycast(transform.position, dir, distance,layerCheck);

        if(raycast.collider != null)
        {
            // Xử lí bắn trúng Enemy.
        }

        m_previewPos = transform.position;
    }
}

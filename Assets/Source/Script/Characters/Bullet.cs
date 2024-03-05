using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public LayerMask layerCheck;

    public float moveSpeed;

    private Actor m_owner;
    private GunWeapon m_weapon;
    private Vector2 m_bulletDir;
    private Vector3 m_previewPos;

    public Vector2 BulletDir { get => m_bulletDir; set => m_bulletDir = value; }
    public Actor Owner { get => m_owner; set => m_owner = value; }
    public GunWeapon Weapon { get => m_weapon; set => m_weapon = value; }

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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, distance, layerCheck);
        Collider2D col = hit.collider;
        if (col != null)
        {
            if (col.gameObject.CompareTag(GameTag.Enemy.ToString()) || col.gameObject.CompareTag(GameTag.Ground.ToString()))
            {
                Enemy enemy = col.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(m_owner, m_weapon.gunStat.damage);
                }
                else
                {
                    Debug.Log(3);
                }

                gameObject.SetActive(false);
            }
        }

        m_previewPos = transform.position;
    }
}

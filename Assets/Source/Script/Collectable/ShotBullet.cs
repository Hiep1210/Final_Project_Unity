using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class ShotBullet : MonoBehaviour
{
    private GameObject m_gunObj;
    private ShotGun m_shotGun;
    private Vector3 m_bulletDir;
    private int m_indexBullet;
    private float m_valueExtraDir;
    private Vector3 m_newBulletDir;

    private CircleCollider2D m_circleCollider;
    private Rigidbody2D m_rb;

    public Vector3 BulletDir { get => m_bulletDir; set => m_bulletDir = value; }
    public GameObject GunObj { get => m_gunObj; set => m_gunObj = value; }
    public int IndexBullet { get => m_indexBullet; set => m_indexBullet = value; }

    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_circleCollider = GetComponent<CircleCollider2D>();

        m_rb.isKinematic = true;
        //m_circleCollider.isTrigger = true;

        m_shotGun = m_gunObj.gameObject.GetComponent<ShotGun>();

        if (m_shotGun != null)
        {
            m_bulletDir = m_shotGun.mouseToWoldPos - m_shotGun.transform.position;

            m_valueExtraDir = 8;

            switch (m_indexBullet)
            {
                case 1:
                    m_newBulletDir = new Vector3(m_bulletDir.x, m_bulletDir.y - m_valueExtraDir, m_bulletDir.z);
                    break;
                case 2:
                    m_newBulletDir = m_bulletDir;
                    break;
                case 3:
                    m_newBulletDir = new Vector3(m_bulletDir.x, m_bulletDir.y + m_valueExtraDir, m_bulletDir.z);
                    break;
                default:
                    Console.WriteLine("Gan Index Shot-Bullet.");
                    break;
            }

            m_bulletDir = m_newBulletDir;
            m_bulletDir.Normalize();
        }
    }

    private void Update()
    {
        if (m_gunObj == null || m_shotGun == null) return;

        Vector3 newPosition = transform.position + (m_bulletDir * m_shotGun.shotGunStat.flyBulletSpeed * Time.deltaTime);
        newPosition.z = transform.position.z;
        transform.position = newPosition;
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag(GameTag.Enemy.ToString()))
    //    {
    //        Actor enemyActor = collision.GetComponent<Actor>();

    //        if (enemyActor != null)
    //        {
    //            enemyActor.TakeDamage(m_shotGun, m_shotGun.shotGunStat.damage);
    //            //gameObject.SetActive(false);
    //        }
    //    }
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(GameTag.Enemy.ToString()))
        {
            Actor actorEnemy = collision.gameObject.GetComponent<Actor>();

            if (actorEnemy != null)
            {
                Vector3 enemyToBulletDir = transform.position - actorEnemy.transform.position;
                enemyToBulletDir.Normalize();

                actorEnemy.Rb2D.velocity -= (Vector2)enemyToBulletDir * m_shotGun.shotGunStat.recoil;

                actorEnemy.TakeDamage(m_shotGun, m_shotGun.shotGunStat.damage);

            }
            
        }
    }
}

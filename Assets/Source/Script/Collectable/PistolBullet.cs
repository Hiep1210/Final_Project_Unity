using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PistolBullet : MonoBehaviour
{
    private GameObject m_gunObj;
    private PistolGun m_pistolGun;
    private Vector3 m_bulletDir;

    private CircleCollider2D m_circleCollider;
    private Rigidbody2D m_rb;

    public Vector3 BulletDir { get => m_bulletDir; set => m_bulletDir = value; }
    public GameObject GunObj { get => m_gunObj; set => m_gunObj = value; }

    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_circleCollider = GetComponent<CircleCollider2D>();

        m_rb.isKinematic = true;
        m_circleCollider.isTrigger = true;

        m_pistolGun = m_gunObj.gameObject.GetComponent<PistolGun>();

        if (m_pistolGun != null)
        {
            m_bulletDir = m_pistolGun.mouseToWoldPos - m_pistolGun.transform.position;
            m_bulletDir.Normalize();
        }
    }

    private void Update()
    {
        if (m_gunObj == null || m_pistolGun == null) return;

        Vector3 newPosition = transform.position + (m_bulletDir * m_pistolGun.pistolGunStat.flyBulletSpeed * Time.deltaTime);
        newPosition.z = transform.position.z;
        transform.position = newPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameTag.Enemy.ToString()))
        {
            Actor enemyActor = collision.GetComponent<Actor>();

            if (enemyActor != null)
            {
                enemyActor.TakeDamage(m_pistolGun, m_pistolGun.pistolGunStat.damage);
                gameObject.SetActive(false);
            }
        }
    }
}

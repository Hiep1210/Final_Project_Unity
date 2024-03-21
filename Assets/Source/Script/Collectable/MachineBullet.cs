using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class MachineBullet : MonoBehaviour
{
    private GameObject m_gunObj;
    private MachineGun m_machineGun;
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

        m_machineGun = m_gunObj.gameObject.GetComponent<MachineGun>();

        if (m_machineGun != null)
        {
            m_bulletDir = m_machineGun.mouseToWoldPos - m_machineGun.transform.position;
            m_bulletDir.Normalize();
        }
    }

    private void Update()
    {
        if (m_gunObj == null || m_machineGun == null) return;

        Vector3 newPosition = transform.position + (m_bulletDir * m_machineGun.machineGunStat.flyBulletSpeed * Time.deltaTime);
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
                enemyActor.TakeDamage(m_machineGun, m_machineGun.machineGunStat.damage);
                gameObject.SetActive(false);
            }
        }
    }
}

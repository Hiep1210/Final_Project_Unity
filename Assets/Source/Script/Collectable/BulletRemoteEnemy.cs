using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRemoteEnemy : MonoBehaviour
{
    private Actor m_actorOwner;
    private Player m_player;
    private float m_flyBulletSpeed;
    private Vector3 m_bulletDir;

    public Player Player { get => m_player; set => m_player = value; }
    public float FlySpeed { get => m_flyBulletSpeed; set => m_flyBulletSpeed = value; }
    public Actor ActorOwner { get => m_actorOwner; set => m_actorOwner = value; }


    // Start is called before the first frame update
    void Start()
    {
        m_bulletDir = m_player.transform.position - m_actorOwner.transform.position;
        m_bulletDir.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_player != null && m_actorOwner != null)
        {
            transform.Translate(m_bulletDir * FlySpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameTag.Player.ToString()))
        {
            Actor targetActor = collision.gameObject.GetComponent<Player>();
            if (targetActor != null)
            {
                targetActor.TakeDamage(m_actorOwner, m_actorOwner.actorStat.Damage);
                gameObject.SetActive(false);
            }
        }
    }
}

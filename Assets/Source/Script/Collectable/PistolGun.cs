using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolGun : MonoBehaviour
{
    public GameObject playerGameObject;
    public SpriteRenderer m_graphicSp;

    public GunStat pistolGunStat;
    public GameObject pistolBulletPrefabs;
    public Transform bulletSpawnPos;
    public GameObject muzzletVfxPrefabs;

    [HideInInspector]
    public Vector3 mouseToWoldPos;


    private SpriteRenderer m_gunSp;
    private Player m_player;

    private bool m_isAttacked;
    private float m_currTimeAttack;

    private void Awake()
    {
        m_player = playerGameObject.gameObject.GetComponent<Player>();
        m_gunSp = GetComponent<SpriteRenderer>();

        m_gunSp.sortingOrder = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_player == null || m_player.IsDeadActor) return;

        GetMouseDirection();
        GunRotation();
        FireBulletGunChecking();
    }

    private void GetMouseDirection()
    {
        Vector3 mousePosition = Input.mousePosition;
        mouseToWoldPos = Camera.main.ScreenToWorldPoint(mousePosition);
    }

    private void GunRotation()
    {
        Vector3 targetMouseDir = mouseToWoldPos - transform.position;
        targetMouseDir.Normalize();

        float angle = Mathf.Atan2(targetMouseDir.y, targetMouseDir.x) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = rotation;

        float angleEuler = transform.eulerAngles.z;

        if (angleEuler > 90 && angleEuler < 270)
        {
            m_gunSp.transform.localScale = new Vector3(1, -1f, 0);
        }
        else
        {
            m_gunSp.transform.localScale = new Vector3(1, 1, 0);
        }

        if(angleEuler > 45 && angleEuler < 135)
        {
            m_gunSp.sortingOrder = 1;
        }
        else
        {
            m_gunSp.sortingOrder = 3;
        }


        if (m_graphicSp.transform.localScale.x > 0)
        {
            transform.position = new Vector3(m_player.transform.position.x + 2.6f, m_player.transform.position.y - 6f, m_player.transform.position.z + 0f);
        }
        else if (m_graphicSp.transform.localScale.x < 0)
        {
            transform.position = new Vector3(m_player.transform.position.x - 2.6f, m_player.transform.position.y - 6f, m_player.transform.position.z + 0f);
        }
    }

    private void FireBulletGunChecking()
    {
        if (GamepadController.Ins.IsFreeBullet && !m_isAttacked)
        {
            FireBullet();
            m_isAttacked = true;
        }

        if (m_isAttacked)
        {
            m_player.ReduceTimeAction(ref m_isAttacked, ref m_currTimeAttack, pistolGunStat.rateTimeFireBullet);
        }
    }

    private void FireBullet()
    {
        if (muzzletVfxPrefabs != null)
        {
            GameObject muzzletObj = GameObject.Instantiate(muzzletVfxPrefabs, bulletSpawnPos.position, Quaternion.identity);
            Destroy(muzzletObj.gameObject, 0.1f);
        }

        if (pistolBulletPrefabs != null)
        {
            GameObject bulletClone = GameObject.Instantiate(pistolBulletPrefabs, bulletSpawnPos.position, Quaternion.identity);
            PistolBullet pistolBullet = bulletClone.gameObject.GetComponent<PistolBullet>();
            pistolBullet.GunObj = gameObject;
            Rigidbody2D rb = pistolBullet.GetComponent<Rigidbody2D>();
            //rb.AddForce(transform.right * gunPistolStat.bulletForce, ForceMode2D.Impulse);      
        }
    }

}

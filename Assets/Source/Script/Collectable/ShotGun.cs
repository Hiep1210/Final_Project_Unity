using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : MonoBehaviour
{
    public GameObject playerGameObject;
    public SpriteRenderer m_graphicSp;

    public GunStat shotGunStat;
    public GameObject shotBulletPrefabs;
    public Transform bulletSpawnPos;
    public GameObject muzzletVfxPrefabs;

    [HideInInspector]
    public Vector3 mouseToWoldPos;

    private SpriteRenderer m_gunSp;
    private Player m_player;

    private bool m_isAttacked;
    private float m_currTimeAttack;

    private int m_countBullet;
    public int CountBullet { get => m_countBullet; set => m_countBullet = value; }
    private void Awake()
    {
        m_player = playerGameObject.gameObject.GetComponent<Player>();
        m_gunSp = GetComponent<SpriteRenderer>();

        m_gunSp.sortingOrder = 3;

        m_countBullet = shotGunStat.countBullet;
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

        if (angleEuler > 40 && angleEuler < 135)
        {
            m_gunSp.sortingOrder = 1;
        }
        else
        {
            m_gunSp.sortingOrder = 3;
        }


        if (m_graphicSp.transform.localScale.x > 0)
        {
            transform.position = new Vector3(m_player.transform.position.x, m_player.transform.position.y - 6.8f, m_player.transform.position.z + 0f);
        }
        else if (m_graphicSp.transform.localScale.x < 0)
        {
            transform.position = new Vector3(m_player.transform.position.x, m_player.transform.position.y - 6.8f, m_player.transform.position.z + 0f);
        }
    }

    private void FireBulletGunChecking()
    {
        if (m_isAttacked)
        {
            m_player.ReduceTimeAction(ref m_isAttacked, ref m_currTimeAttack, shotGunStat.rateTimeFireBullet);
        }

        if (GamepadController.Ins.IsFreeBullet && !m_isAttacked && m_countBullet > 0)
        {
            m_isAttacked = true;
            FireBullet();
        }
    }

    private void FireBullet()
    {
        if (muzzletVfxPrefabs != null)
        {
            GameObject muzzletObj = GameObject.Instantiate(muzzletVfxPrefabs, bulletSpawnPos.position, Quaternion.identity);
            Destroy(muzzletObj.gameObject, 0.1f);
        }

        if (shotBulletPrefabs != null)
        {
            GameObject bulletClone1 = GameObject.Instantiate(shotBulletPrefabs, bulletSpawnPos.position, Quaternion.identity);
            ShotBullet shotBullet1 = bulletClone1.gameObject.GetComponent<ShotBullet>();
            shotBullet1.GunObj = gameObject;
            shotBullet1.IndexBullet = 1;


            GameObject bulletClone2 = GameObject.Instantiate(shotBulletPrefabs, bulletSpawnPos.position, Quaternion.identity);
            ShotBullet shotBullet2 = bulletClone2.gameObject.GetComponent<ShotBullet>();
            shotBullet2.GunObj = gameObject;
            shotBullet2.IndexBullet = 2;

            GameObject bulletClone3 = GameObject.Instantiate(shotBulletPrefabs, bulletSpawnPos.position, Quaternion.identity);
            ShotBullet shotBullet3 = bulletClone3.gameObject.GetComponent<ShotBullet>();
            shotBullet3.GunObj = gameObject;
            shotBullet3.IndexBullet = 3;

            m_countBullet -= 1;
        }
    }
}

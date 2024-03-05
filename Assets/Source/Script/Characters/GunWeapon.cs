using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunWeapon : MonoBehaviour
{
    public Player player;
    public GunStat gunStat;
    public Bullet bullet;
    public SpriteRenderer spPlayer;
    public Rigidbody2D rbPlayer;

    private Animator m_animator;
    private Vector3 targetMouseDir;

    private bool m_isFireBulleted;
    private float m_currentFireBullet;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (player.IsDead) return;
        GetGunToAimMouseDir();
        GunRotation();
        FireBulletChecking();
    }

    private void GetGunToAimMouseDir()
    {
        Vector3 mousePouse = Input.mousePosition;
        Vector3 camWorldPosToMousePos = Camera.main.ScreenToWorldPoint(mousePouse);
        targetMouseDir = camWorldPosToMousePos - transform.position;
        targetMouseDir.Normalize();
    }

    private void GunRotation()
    {
        float angle = Mathf.Atan2(targetMouseDir.y, targetMouseDir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);

        float eulerAngles = transform.eulerAngles.z;

        if (eulerAngles < 270 && eulerAngles > 90)
        {
            transform.localScale = new Vector3(1, -1f, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (spPlayer.transform.localScale.x > 0)
        {
            transform.position = new Vector3(player.transform.position.x - 0.45f, player.transform.position.y + 0.6f, player.transform.position.z + 0f);
        }
        else
        {
            transform.position = new Vector3(player.transform.position.x + 0.45f, player.transform.position.y + 0.6f, player.transform.position.z + 0f);
        }

    }

    private void FireBulletChecking()
    {
        ReduceActionTime(ref m_isFireBulleted, ref m_currentFireBullet, gunStat.timeRate);

        if (GamepadController.Ins.IsFireBulletAttack)
        {
            if (!m_isFireBulleted)
            {
                FireBulletAttack(targetMouseDir);
                m_isFireBulleted = true;
            }
        }
    }

    private void FireBulletAttack(Vector3 dir)
    {
        if (bullet == null) return;

        m_animator.SetBool("FireBullet", true);
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + 0.15f, transform.position.z);
        GameObject bulletClone = Instantiate(bullet.gameObject, spawnPos, Quaternion.identity);
        Bullet b = bulletClone.GetComponent<Bullet>();
        b.BulletDir = dir;
        b.Owner = player;
        b.Weapon = this;

        rbPlayer.velocity = -gunStat.recoil * dir;

        //if (spPlayer.transform.localScale.x > 0)
        //{
        //    rbPlayer.velocity = new Vector2(-gunStat.recoil, rbPlayer.velocity.y);
        //}
        //else
        //{
        //    rbPlayer.velocity = new Vector2(gunStat.recoil, rbPlayer.velocity.y);
        //}
    }

    private void ResetAttackAnimation()
    {
        rbPlayer.velocity = Vector2.zero;
        m_animator.SetBool("FireBullet", false);
    }

    protected void ReduceActionTime(ref bool isAction, ref float currentTimeAction, float timeRateAction)
    {
        if (!isAction) return;

        currentTimeAction += Time.deltaTime;
        if (currentTimeAction >= timeRateAction)
        {
            currentTimeAction = 0;
            isAction = false;
        }
    }
}

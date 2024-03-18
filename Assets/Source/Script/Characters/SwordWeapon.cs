using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWeapon : MonoBehaviour
{
    public Player player;
    public SwordStat swordStat;
    public SpriteRenderer spPlayer;

    private Animator m_animator;
    private Vector3 targetMouseDir;

    private bool m_swordAttacked;
    private float m_currentSword;

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
        SwordRotation();
        SwordAttackChecking();
    }


    private void SwordRotation()
    {
        if (spPlayer.transform.localScale.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, -50f);
            transform.position = new Vector3(player.transform.position.x + 0.65f, player.transform.position.y + 1.45f, player.transform.position.z + 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, -180, -50f);
            transform.position = new Vector3(player.transform.position.x - 0.65f, player.transform.position.y + 1.45f, player.transform.position.z + 0f);
        }
    }

    private void SwordAttackChecking()
    {
        ReduceActionTime(ref m_swordAttacked, ref m_currentSword, swordStat.timeRate);

        if (GamepadController.Ins.IsSwordAttack)
        {
            if (!m_swordAttacked)
            {
                SwordAttack();
                m_swordAttacked = true;
            }
        }
    }

    private void SwordAttack()
    {
        m_animator.SetBool("SwordAttack", true);
    }

    private void ResetAttackAnimation()
    {
        m_animator.SetBool("SwordAttack", false);
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

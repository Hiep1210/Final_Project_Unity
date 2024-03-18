using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    public Player playerTarget;

    public MethodDetect typeDetect;

    [Header("Setting Detect: ")]
    public LayerMask layerCheck;
    public float radiusCheck;
    public float radiusLocalCheck;
    public Vector3 offset;

    private bool m_detectChassing;
    private bool m_detectAttack;

    public bool DetectChassing { get => m_detectChassing; }
    public bool DetectAttack { get => m_detectAttack; }

    private void FixedUpdate()
    {
        if (playerTarget != null)
        {
            DetectChassingChecking();
            DetectAttackChecking();
        }
    }

    private void DetectChassingChecking()
    {
        Collider2D col = Physics2D.OverlapCircle(transform.position + offset, radiusCheck, layerCheck);
        if (col != null)
        {
            m_detectChassing = true;
        }
        else
        {
            m_detectChassing = false;
        }
    }

    private void DetectAttackChecking()
    {
        if (!m_detectChassing) return;

        Collider2D col = Physics2D.OverlapCircle(transform.position + offset, radiusLocalCheck, layerCheck);
        if (col != null)
        {
            m_detectAttack = true;
        }
        else
        {
            m_detectAttack = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + offset, radiusCheck);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + offset, radiusLocalCheck);
    }
}

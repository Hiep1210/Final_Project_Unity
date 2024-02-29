using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    public Player targetDetect;
    public MethodDetect methodDetect;
    [Header("Setting-OverlapCircle-Detect: ")]
    public LayerMask layerOverlapCheck;
    public float radiusCheck;
    public Vector3 offset;

    [Header("Setting-OverlapCircle-Detect: ")]
    public LayerMask layerRaycastCheck;
    public float distanceCheck;

    private bool m_isDetected;

    public bool IsDetected { get => m_isDetected; }

    // Update is called once per frame
    void Update()
    {
        DetectPlayerChecking();
    }

    private void DetectPlayerChecking()
    {
        if (targetDetect != null)
        {
            if (methodDetect == MethodDetect.Raycast)
            {
                Vector3 targetDir = targetDetect.transform.position - transform.position;
                targetDir.Normalize();
                RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDir, distanceCheck, layerRaycastCheck);
                if (hit.collider != null)
                {
                    m_isDetected = true;
                }
                else
                {
                    m_isDetected = false;
                }

            }
            else if (methodDetect == MethodDetect.OverlapCircle)
            {
                Collider2D col = Physics2D.OverlapCircle(transform.position + offset, radiusCheck, layerOverlapCheck);
                if (col != null)
                {
                    m_isDetected = true;
                }
                else
                {
                    m_isDetected = false;
                }
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (methodDetect == MethodDetect.OverlapCircle)
        {
            Gizmos.DrawWireSphere(transform.position + offset, radiusCheck);
        }
        else if (methodDetect == MethodDetect.Raycast)
        {
            Vector3 targetDir = targetDetect.transform.position - transform.position;
            targetDir.Normalize();
            Vector3 endPos = transform.position + (targetDir * distanceCheck);
            Gizmos.DrawLine(transform.position, endPos);
        }
    }
}

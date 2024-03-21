using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectTarget : MonoBehaviour
{
    [Header("Stat Overlap Check - Walk: ")]
    public LayerMask layerCheckWalk;
    public float radiusCheckWalk;
    public Vector3 offSetCheckWalk;

    [Header("Stat Overlap Check - Chassing: ")]
    public LayerMask layerCheckChassing;
    public float radiusCheckChassing;
    public Vector3 offSetCheckChassing;

    private bool m_isDetectedWalk;
    private bool m_isDetectedChassing;

    public bool IsDetectedWalk { get => m_isDetectedWalk; set => m_isDetectedWalk = value; }
    public bool IsDetectedChassing { get => m_isDetectedChassing; set => m_isDetectedChassing = value; }

    private void FixedUpdate()
    {

        OverLapChecking(offSetCheckWalk, radiusCheckWalk, layerCheckWalk, ref m_isDetectedWalk);
        OverLapChecking(offSetCheckChassing, radiusCheckChassing, layerCheckChassing, ref m_isDetectedChassing);
    }

    private void OverLapChecking(Vector3 offSet, float radiusCheck, LayerMask layerCheck, ref bool isDetect)
    {
        Collider2D col = Physics2D.OverlapCircle(transform.position + offSet, radiusCheck, layerCheck);
        if (col != null)
        {
            isDetect = true;
        }
        else
        {
            isDetect = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Helper.ChangAlpha(Color.green,0.1f);
        Gizmos.DrawWireSphere(transform.position + offSetCheckWalk, radiusCheckWalk);
        Gizmos.color = Helper.ChangAlpha(Color.red, 0.1f);
        Gizmos.DrawWireSphere(transform.position + offSetCheckChassing, radiusCheckChassing);
    }

}

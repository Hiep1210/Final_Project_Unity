using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T m_ins;

    public static T Ins { get { return m_ins; } }


    public virtual void Awake()
    {
        MakeSingleton(true);
    }

    public void MakeSingleton(bool destroyOnload)
    {
        if (m_ins == null)
        {
            m_ins = this as T;
            if (destroyOnload)
            {
                DontDestroyOnLoad(this.gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

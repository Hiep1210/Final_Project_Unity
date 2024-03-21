using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public GameObject destroyCollectableVfx;

    public float minValueCollectable;
    public float maxValueCollectable;

    protected float m_valueRandom;

    private void Awake()
    {
        
    }

    protected virtual void HandlerEvent(Player player)
    {
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameTag.Player.ToString()))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if(player != null)
            {
                m_valueRandom = UnityEngine.Random.Range(minValueCollectable, maxValueCollectable);

                HandlerEvent(player);

                if (destroyCollectableVfx)
                {
                    GameObject destroyClone = Instantiate(destroyCollectableVfx.gameObject, transform.position, Quaternion.identity);
                    Destroy(destroyClone, 1.5f);
                }

                gameObject.SetActive(true);
                Destroy(gameObject, 0.1f);
            }
        }
    }
}

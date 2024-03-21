using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    public Animator anim;
    public GameObject target;
    public float speed =15f ;
    Vector2 moveTo;
    float timeOfStop;
    // Start is called before the first frame update
    void Start()
    {
        //timeOfStop = anim.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        //anim.Play("stop");
        anim = GetComponent<Animator>();
        //Debug.Log(timeOfStop);
        moveTo = target.transform.position;
        StartCoroutine(StartAnimation("stop"));
    }

    private IEnumerator StartAnimation(string animName)
    {
        Debug.Log("Run");
        yield return new WaitForSeconds(3f);
        anim.SetBool("stopdecision", true);

    }

    // Update is called once per frame
    void Update()
    {
        if((Vector2)transform.position == moveTo)
        {
            anim.SetBool("stopdecision", false);
            StartCoroutine(StartAnimation("stop"));
            moveTo = target.transform.position;
        }
        turn(moveTo);
        anim.Play("huc");
        transform.position = Vector2.MoveTowards(transform.position, moveTo, speed * Time.deltaTime);
        //StartCoroutine(Attack(moveTo));
        
    }
    private void turn(Vector3 place)
    {
        Vector3 dir = place - transform.position;
        dir.Normalize();
        if((dir.x < 0))
        {
            if (transform.localScale.x > 0)
                transform.localScale = new Vector3(-1,1,1);
        }
        else
        {
            if (transform.localScale.x < 0)
                transform.localScale = new Vector3(1,1,1);
        }

    }
}

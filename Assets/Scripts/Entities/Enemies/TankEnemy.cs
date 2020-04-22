using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankEnemy : Enemy
{
    public float rangeOfGrab;
    private void Awake()
    {
        entitiesAnimator = gameObject.GetComponent<Animator>();
        entitiesAnimator.updateMode = UnityEngine.AnimatorUpdateMode.Normal;
        base.Start();

        timeTillNextGrab = timeUntillGrab + Time.time;
    }
    

    // Update is called once per frame
    void Update()
    {
        if(!froze)
        {
            if (Vector3.Distance(player.position, transform.position) <= rangeOfGrab && Time.time >= timeTillNextGrab)
            {
                entitiesAnimator.SetBool("Walk", false);
                StartCoroutine(Grab());
                entitiesAnimator.SetTrigger("Attack");
            }
            else
            {
                SB("pursue");
                entitiesAnimator.SetBool("Walk", true);
            }
        }
        
        

        if (dead)
        {
            Destroy(this.gameObject);
        }

    }
}

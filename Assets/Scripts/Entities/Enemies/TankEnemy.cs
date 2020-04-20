using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankEnemy : Enemy
{
    public float rangeOfGrab;

    // Start is called before the first frame update
    protected override void Start()
    {
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
                StartCoroutine(Grab());
            }
            else
            {
                SB("persue");

            }
        }
        

        if (dead)
        {
            Destroy(this.gameObject);
        }

    }
}

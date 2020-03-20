using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankEnemy : Enemy
{
    public float rangeOfGrab;
    public float grabTime;

    public float timeUntillGrab;
    float timeTillNextGrab;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        timeTillNextGrab = timeUntillGrab + Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Vector3.Distance(player.position, transform.position) <= rangeOfGrab && Time.time >= timeTillNextGrab)
        {
            //StartCoroutine(Attack());
        }
        else
        {
            agent.SetDestination(player.position);

        }

        if (dead)
        {
            Destroy(this.gameObject);
        }

    }

    IEnumerator Grab()
    {
        agent.enabled = false;

        Vector3 startingAttackPosition = transform.position;
        Vector3 dirToTarget = (player.position - transform.position).normalized;
        Vector3 attackPosition = player.position - dirToTarget;

        float percent = 0;

        bool hasGrabbed = false;


        while(percent <= 1)
        {

            if(percent >= .5f && !hasGrabbed)
            {
                player.GetComponent<NavMeshAgent>().enabled = false;

                hasGrabbed = true;
            }

            transform.position = Vector3.Lerp(startingAttackPosition, attackPosition, 2);

            percent += Time.deltaTime * grabTime;
            yield return null;

        }

        player.GetComponent<NavMeshAgent>().enabled = true;

        agent.enabled = true;

        timeTillNextGrab = timeUntillGrab + Time.time;

    }
}

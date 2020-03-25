using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PawnScript : Enemy
{
    public PawnSpawnerScript pawnSpawner;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        timeTillNextAttack = Time.time + timeBetweenAttack;

    }

    // Update is called once per frame
    void Update()
    {
        if(!froze)
        {
            if (attacking == false)
            {
                agent.enabled = true;
                SB("seek");

            }

            if (attacking == false && Vector3.Distance(transform.position, player.position) < meleeAttackRange)
            {
                if (Time.time >= timeTillNextAttack)
                {
                    StartCoroutine(base.MeleeAttack());
                }
            }
        }
        
        if(dead || health < 0)
        {
            if(pawnSpawner != null)
                pawnSpawner.PawnDied();

            Destroy(this.gameObject);
        }
        
    }
}

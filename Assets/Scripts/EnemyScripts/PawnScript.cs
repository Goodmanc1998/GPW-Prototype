using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PawnScript : Entities
{
    

    public PawnSpawnerScript pawnSpawner;

    bool attacking;
    public float meleeAttackRange;
    public float meleeAttackDamage;
    public float meleeAttackSpeed;
    public float timeBetweenAttack;
    float timeTillNextAttack;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        timeTillNextAttack = Time.time + timeBetweenAttack;

    }

    // Update is called once per frame
    void Update()
    {
        if(attacking == false)
        {
            agent.SetDestination(player.position);
        }

        if (attacking == false && Vector3.Distance(transform.position, player.position) < meleeAttackRange)
        {
            if(Time.time >= timeTillNextAttack)
            {
                StartCoroutine(Attack());
            }
        }

        if(dead)
        {
            pawnSpawner.PawnDied();
        }
        
    }

    IEnumerator Attack()
    {
        agent.enabled = false;
        attacking = true;
        Debug.Log("Attacking");

        Vector3 startingAttackPosition = transform.position;
        Vector3 dirToTarget = (player.position - transform.position).normalized;
        Vector3 attackPosition = player.position - dirToTarget;

        float percent = 0;

        bool hasAppliedDamage = false;

        while (percent <= 1)
        {

            if (percent >= .5f && !hasAppliedDamage)
            {
                hasAppliedDamage = true;
                player.gameObject.GetComponent<PlayerMovement>().TakeDamage(meleeAttackDamage, "Melee");
            }

            percent += Time.deltaTime * meleeAttackSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector3.Lerp(startingAttackPosition, attackPosition, interpolation);

            yield return null;
        }

        timeTillNextAttack = Time.time + timeBetweenAttack;

        attacking = false;
        agent.enabled = true;
    }

    
}

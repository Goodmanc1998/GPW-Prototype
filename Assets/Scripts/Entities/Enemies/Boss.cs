using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : Enemy
{
    //Floats used to store the max and min shooting range
    public float minShootRange;
    public float maxShootRange;
    bool ranagedAttack;

    float distToPlayer;

    bool dashAttack;

    bool phaseOne;

    bool bossAttack;
    bool flee;

    public int amountofHits;
    int currentHits;

    Transform attackPoint;
    Transform returnPoint;
    GameObject block;

    public GameObject bossSpell;


    protected override void Start()
    {
        base.Start();

        attackPoint = GameObject.FindGameObjectWithTag("attackPoint").transform;
        returnPoint = GameObject.FindGameObjectWithTag("returnPoint").transform;
        block = GameObject.FindGameObjectWithTag("block");

    }
    private void Update()
    {
        //fight starts
        distToPlayer = Vector3.Distance(player.position, transform.position); // Calculates the distance between the player and enemy

        if(health > startingHealth / 100 * 33)
        {
            phaseOne = true;
        }
        else
        {
            phaseOne = false;
        }

        if(!dead && !bossAttack)
        {

            if(phaseOne || !phaseOne)
            {
                if(currentHits >= amountofHits && !bossAttack)
                {
                    StartCoroutine(FleeShootAttack());
                }


                if(distToPlayer > maxShootRange)
                {
                    if(!bossAttack)
                    {
                        StartCoroutine(DashAttack());
                    }
                }
                else if(distToPlayer < maxShootRange && distToPlayer > minShootRange)
                {
                    float rndNo = Random.Range(0, 100);

                    if(!bossAttack && rndNo < 75)
                    {
                        StartCoroutine(RangedAttack());
                    }
                }
                else if(distToPlayer < meleeAttackRange)
                {
                    float rndNo = Random.Range(0, 100);

                    if (!attacking && !bossAttack && rndNo < 75)
                    {
                        StartCoroutine(MeleeAttack());
                    }

                }
            }




        }

        /*

        if(seenPlayer == true)
        {
            if (!froze)
            {
                //health is > 33%
                if (health > startingHealth / 100 * 33)
                {

                    if (!froze)
                    {
                        //If the player is outside Melee & shoot range persue player
                        if(distance > maxShootRange && distance > meleeAttackRange)
                        {
                            SB("persue");
                        }
                        else if(distance < meleeAttackRange)
                        {
                            if(!dashAttack)
                                StartCoroutine(DashAttack());

                        }

                        

                        //keep distance to player 
                        // If the enemy is out of shooting range, move towards the player
                        if (distance > shootRange && distance > meleeAttackRange)
                        {
                            SB("persue");
                        }
                        // If the enemy gets within range of the player, shoot at them
                        else 
                        {



                            //dashes in to melee attack flurry of 4 strikes
                            MeleeAttack();
                            MeleeAttack();
                            MeleeAttack();
                            MeleeAttack();
                            //froze in place take double damage.
                            SB("freeze");

                            SB("flee"); // Effectively stops the enemy from moving
                        }

                    



                    }
                }
                else if (health <= startingHealth / 100 * 33)
                {
                    //health is < 33%

                    //keep distance to player 
                    // If the enemy is out of shooting range, move towards the player
                    if (distance > maxShootRange)
                    {
                        SB("persue");
                    }
                    // If the enemy gets within range of the player, shoot at them
                    else if (distance < maxShootRange)
                    {
                        //dashes in to melee attack flurry of 4 strikes
                        Shoot();
                        shot += 1;
                        // range attacks and special attacks in sequence
                        if (shot >= 4)
                        {
                            //after special attack froze in place take double damage.
                            SB("freeze");
                            shot = 0;
                        }

                        SB("flee"); // Effectively stops the enemy from moving
                    }


                }
            }
        }
       
        */
        

    }

    IEnumerator DashAttack()
    {

        Debug.Log("Dash Attack");

        //Setting Dash Attack to true
        bossAttack = true;

        //If the player is still outside the melee range see
        if(distToPlayer > meleeAttackRange)
        {
            SB("seek");

            //Updating the movement starts for the Boss to chase player
            UpdateMovementStats(movementSpeed + 5, angularSpeed + 180, acceleration + 5);

        }

        //Waits till the player is within melee range
        yield return new WaitUntil(() => distToPlayer < meleeAttackRange);

        //For loop to run for as many melee attacks
        for (int i = 0; i < 2; i++)
        {
            //Resseting the movement stats
            ResetMovementStats();

            //Starting the melee attack
            StartCoroutine(MeleeAttack());

            //Setting up the next attack time
            timeTillNextAttack = Time.time + timeBetweenAttack;

            //Waiting until attack is finished and can attack again
            yield return new WaitUntil(() => attacking == false && Time.time > timeTillNextAttack);

        }

        bossAttack = false;

    }

    IEnumerator RangedAttack()
    {

        Debug.Log("Ranged Attack");

        bossAttack = true;

        SB("flee");

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 2; i++)
        {
            Vector3 lookDir = player.transform.position - transform.position;

            Quaternion lookRotation = Quaternion.LookRotation(lookDir);

            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * agent.angularSpeed).eulerAngles;

            transform.rotation = Quaternion.Euler(0.0f, rotation.y, 0.0f);

            StartCoroutine(Shoot());

            yield return new WaitForSeconds(1f);

        }

        bossAttack = false;

    }

    IEnumerator FleeShootAttack()
    {
        Debug.Log("Flee Shoot Attack");

        bossAttack = true;


        agent.SetDestination(attackPoint.position);

        yield return new WaitUntil(() => Vector3.Distance(transform.position, attackPoint.position) <= 1);

        Debug.Log("Reached Point");


        block.GetComponent<NavMeshObstacle>().enabled = true;

        Debug.Log("Block True");


        for (int i = 0; i < 4; i++)
        {
            Vector3 lookDir = player.transform.position - transform.position;

            Quaternion lookRotation = Quaternion.LookRotation(lookDir);

            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * agent.angularSpeed).eulerAngles;

            transform.rotation = Quaternion.Euler(0.0f, rotation.y, 0.0f);

            yield return new WaitForSeconds(1f);

            Instantiate(bossSpell, transform.position + ((transform.forward * 2) + (transform.up * 2)), transform.rotation);

        }

        yield return new WaitForSeconds(3f);


        Debug.Log("Fin Shoot");


        block.GetComponent<NavMeshObstacle>().enabled = false;

        Debug.Log("Block false");

        agent.SetDestination(returnPoint.position);

        yield return new WaitUntil(() => Vector3.Distance(transform.position, attackPoint.position) <= 1);

        Debug.Log("Returned");

        flee = false;

        currentHits = 0;

        bossAttack = false;
    }

    public override void TakeDamage(float damageIn, string attackType)
    {
        base.TakeDamage(damageIn, attackType);

        currentHits++;

        if(currentHits >= amountofHits)
        {
            flee = true;
        }
    }

    void UpdateMovementStats(float newMovement, float newAngular, float newAcceleration)
    {
        agent.speed = newMovement;
        agent.angularSpeed = newAngular;
        agent.acceleration = newAcceleration;
    }

    void ResetMovementStats()
    {
        agent.speed = movementSpeed;
        agent.angularSpeed = angularSpeed;
        agent.acceleration = acceleration;
    }
}

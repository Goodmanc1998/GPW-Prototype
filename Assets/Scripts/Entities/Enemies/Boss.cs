﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;

public class Boss : Enemy
{
    //Floats used to store the max and min shooting range
    public float minShootRange;
    public float maxShootRange;
    bool ranagedAttack;

    float distToPlayer;

    bool bossAttack;
    bool firstFlee;
    bool secondFlee;

    bool takenHitAttack;
    public int amountofHits;
    int currentHits;

    Transform attackPoint;
    Transform returnPoint;
    GameObject block;

    public GameObject bossSpell;

    bool spawnAnimation;

    Rigidbody rb;
    public bool LookPlayer;


    private void Awake()
    {
        entitiesAnimator = gameObject.GetComponent<Animator>();


       


    }

    protected override void Start()
    {
        base.Start();

        attackPoint = GameObject.FindGameObjectWithTag("attackPoint").transform;
        returnPoint = GameObject.FindGameObjectWithTag("returnPoint").transform;
        block = GameObject.FindGameObjectWithTag("block");

        GameObject.FindGameObjectWithTag("UI").GetComponent<UIScript>().boss = this;

        rb = gameObject.GetComponent<Rigidbody>();

        LookPlayer = true;


        spawnAnimation = true;
        StartCoroutine(BossSpawn());
        

    }
    private void Update()
    {
        //fight starts
        distToPlayer = Vector3.Distance(player.position, transform.position); // Calculates the distance between the player and enemy

        if(!dead && !bossAttack && !spawnAnimation)
        {
            if (health < startingHealth / 100 * 66 && !firstFlee)
            {
                StartCoroutine(FleeShootAttack());   
            }

            if (health < startingHealth / 100 * 33 && !secondFlee)
            {
                StartCoroutine(FleeShootAttack());
            }

            if(takenHitAttack && !bossAttack)
            {
                StartCoroutine(DashAttack());

                takenHitAttack = false;
            }

            if(distToPlayer >= maxShootRange && !bossAttack)
            {
                SB("seek");
                //entitiesAnimator.SetBool("Walk", true);
            }

            if(distToPlayer <= maxShootRange && distToPlayer >= minShootRange && !bossAttack)
            {
                StartCoroutine(RangedAttack());
            }

            if(distToPlayer <= meleeAttackRange && !bossAttack)
            {
                if(Time.time > timeTillNextAttack && !attacking)
                {
                    StartCoroutine(BossMelee());

                    entitiesAnimator.SetTrigger("bossMelee");

                }


            }


            if (LookPlayer)
            {
                LookAtPlayer();
            }
            else
            {
                rb.constraints = RigidbodyConstraints.FreezeAll;
            }


        }     
        
       

    }

    void LookAtPlayer()
    {

        transform.LookAt(player.position);
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        Vector3 direction = new Vector3((player.position.x - transform.position.x), 0, (player.position.z - transform.position.z)).normalized;
            //(player.position - transform.position).normalized;

        float step = agent.angularSpeed * Time.deltaTime;

        Vector3 newDir = Vector3.RotateTowards(transform.forward, direction, step, 0.0f);

        transform.rotation = Quaternion.LookRotation(newDir);
    }

    IEnumerator BossMelee()
    {

        bossAttack = true;

        entitiesAnimator.SetTrigger("bossMelee");

        yield return new WaitForSeconds(1f);

        StartCoroutine(MeleeAttack());

        bossAttack = false;


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
            UpdateMovementStats(movementSpeed + 2, angularSpeed + 50, acceleration + 2);


                entitiesAnimator.SetTrigger("bossWalk");


            //Waits till the player is within melee range
            yield return new WaitUntil(() => distToPlayer < meleeAttackRange);

            //Resseting the movement stats
            ResetMovementStats();

        }

        LookPlayer = true;


        //For loop to run for as many melee attacks
        for (int i = 0; i < 2; i++)
        {
            entitiesAnimator.SetTrigger("bossMelee");


            //Starting the melee attack
            StartCoroutine(MeleeAttack());


            //Setting up the next attack time
            timeTillNextAttack = Time.time + 1;

            //Waiting until attack is finished and can attack again
            yield return new WaitUntil(() => attacking == false);

        }

        LookPlayer = false;
        bossAttack = false;

    }

    IEnumerator RangedAttack()
    {

        Debug.Log("Ranged Attack");

        //Setting Boss attack true
        bossAttack = true;

        //Fleeing for X seconds
        SB("flee");

        entitiesAnimator.SetTrigger("bossWalk");
        yield return new WaitForSeconds(2f);


        LookPlayer = true;


        //For loop to shoot X amount 
        for (int i = 0; i < 2; i++)
        {
            //looking at the player

            entitiesAnimator.SetTrigger("bossSpell");

            yield return new WaitForSeconds(1f);

            StartCoroutine(Shoot());

            yield return new WaitForSeconds(1f);

        }

        //Boss attack set to false
        bossAttack = false;
        LookPlayer = false;
    }

    IEnumerator FleeShootAttack()
    {
        Debug.Log("Flee Shoot Attack");

        //Boss attack set to True
        bossAttack = true;

        //Setting the destination to the attack position untill the boss has reached the position
        agent.SetDestination(attackPoint.position);

        entitiesAnimator.SetTrigger("bossWalk");

        yield return new WaitUntil(() => Vector3.Distance(transform.position, attackPoint.position) <= 1);



        //Making the boss inaccessable
        block.GetComponent<NavMeshObstacle>().enabled = true;

        //Boss shoots X amount of spells
        for (int i = 0; i < 8; i++)
        {

            //looking at the player
            LookPlayer = true;

            entitiesAnimator.SetTrigger("bossSpell");

            yield return new WaitForSeconds(1f);

            //Creating the bos spell
            Instantiate(bossSpell, transform.position + ((transform.forward * 2) + (transform.up * 2)), transform.rotation);

            yield return new WaitForSeconds(1f);

        }

        LookPlayer = false;


        //Waiting X seconds after casting spells
        yield return new WaitForSeconds(2f);

        //Disabling the block to allow boss to return to the arena 
        block.GetComponent<NavMeshObstacle>().enabled = false;

        entitiesAnimator.SetTrigger("bossWalk");

        //Moving the boss back to the arena 
        agent.SetDestination(returnPoint.position);
        yield return new WaitUntil(() => Vector3.Distance(transform.position, attackPoint.position) <= 1);

        if(!firstFlee)
        {
            firstFlee = true;

            if (!secondFlee)
                secondFlee = true;
        }


        //Boss attack to false
        bossAttack = false;
    }

    public override void TakeDamage(float damageIn, string attackType)
    {
        base.TakeDamage(damageIn, attackType);

        entitiesAnimator.SetTrigger("bossDamage");

        //Storing the amount of hits 
        currentHits++;

        //Checking if boss should do taken to many hit attack
        if(currentHits >= amountofHits)
        {
            //Setting taken hit attack to true
            takenHitAttack = true;
        }
    }

    IEnumerator BossSpawn()
    {
        //entitiesAnimator.SetBool("bossSpawn", true);

        yield return new WaitForSeconds(5f);

        entitiesAnimator.SetBool("bossSpawn", false);

        spawnAnimation = false;
    }

    //Functions used for updating the bosses movement stats and restting them
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

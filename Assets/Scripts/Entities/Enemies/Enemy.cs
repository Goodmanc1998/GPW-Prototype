using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entities
{
    [Header("Movement")]
    public float movementSpeed;
    public float angularSpeed;
    public float acceleration;
    public float stoppingDistance;

    [Header("Melee Attack")]
    //Variables Used for Melee Attack
    protected bool attacking;
    public float meleeAttackRange;
    public float meleeAttackDamage;
    public float meleeAttackSpeed;
    public float timeBetweenAttack;
    protected float timeTillNextAttack;

    [Header("Spawner")]
    //Variables used for Spawning Pawns
    public PawnScript pawnEnemy;
    public int currentSpawned;
    public float timeBetweenSpawns;
    protected float timeTillNextSpawn;

    [Header("Grab Attack")]
    //Variables used for the Grab Attack
    public float grabTime;
    public float timeUntillGrab;
    protected float timeTillNextGrab;

    [Header("Explosion")]
    //Variables used for the Explosion Attack
    public float explosionRanage;
    public float explosionDamage;
    public float timeUntilExplosion;

    [Header("Ranaged Attack")]
    public Projectile projectile; // The projectile that will be shot by this enemy
    protected Coroutine firing; // Keeps track of the shooting coroutine to ensure a delay between each shot
    public float fireRate; // The time in seconds between each shot projectile

    [HideInInspector]
    public Wave wave; // The wave the enemy was spawned in
    public bool froze = false;

    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = movementSpeed;
        agent.angularSpeed = angularSpeed;
        agent.acceleration = acceleration;
        agent.stoppingDistance = stoppingDistance;
    }

    public override void TakeDamage(float damageIn, string attackType)
    {
        base.TakeDamage(damageIn, attackType);

        // Once the enemy is declared dead, tell the wave an enemy has died if there is one
        if (wave != null && dead)
        {

            if(this.gameObject.GetComponent<PawnScript>() != true)
            {
                wave.EnemyKilled();
            }
        }
    }

    //steering behaviours.
    public virtual void SB(string calledFor)
    {
        NavMeshAgent playerNvAgnt = player.gameObject.GetComponent<NavMeshAgent>();
        //seek
        //flee
        //persue
        //avoid
        if (calledFor == "seek")
        {
            agent.SetDestination(player.position); 
        }else if(calledFor == "flee")
        {
            agent.SetDestination(transform.position - (transform.position + player.position).normalized);
        }else if(calledFor == "persue")
        {
            agent.SetDestination(player.position + playerNvAgnt.velocity);
        }
        else if (calledFor == "stop")
        {
            agent.SetDestination(transform.position);
        }
        else if (calledFor == "evade")
        {
            agent.SetDestination(player.position - playerNvAgnt.velocity);
        }else if (calledFor == "freeze")
        {
            agent.SetDestination(transform.position);
            froze = true;
            for (int i = 0; i < 1320; i++)
            {
                if(i == 1319)
                {
                    froze = false;
                }
            }
        }
    }

    protected void SpawnPawn()
    {
        //Getting the Spawn Position
        Vector3 spawnPosition = transform.position + (transform.forward * 2);

        //Creating the Pawn
        EnemySpawnManager.enemies.Add(Instantiate(pawnEnemy, spawnPosition, transform.rotation));

        //Storing this spawner in the Pawn
        pawnEnemy.pawnSpawner = GetComponent<PawnSpawnerScript>();

        //Increasing the current amount spawned
        currentSpawned++;

        //Setting up time untill next spawn
        timeTillNextSpawn = Time.time + timeBetweenSpawns;

    }

    protected IEnumerator MeleeAttack()
    {
        //Stopping the Agent and starting attack
        agent.enabled = false;
        attacking = true;

        //Getting the start position, direction to player and attack position
        Vector3 startingAttackPosition = transform.position;
        Vector3 dirToTarget = (player.position - transform.position).normalized;
        Vector3 attackPosition;

        if (Vector3.Distance(transform.position, player.position) > meleeAttackRange)
        {
            attackPosition = new Vector3(transform.position.x + (dirToTarget.x * meleeAttackRange), player.position.y + dirToTarget.y, transform.position.z + (dirToTarget.z * meleeAttackRange));

        }
        else
        {
            attackPosition = new Vector3(player.position.x - dirToTarget.x, player.position.y + dirToTarget.y, player.position.z - dirToTarget.z);

        }

        //player.position - dirToTarget;

        float percent = 0;
        bool hasAppliedDamage = false;

        while (percent <= 1)
        {
            //If damage has not been done while over 50% deal damage
            if (percent >= .5f && !hasAppliedDamage)
            {
                hasAppliedDamage = true;
                player.gameObject.GetComponent<PlayerMovement>().TakeDamage(meleeAttackDamage, "Melee");
            }

            //Moving the player between the starting position, and attack position then back to the starting position
            percent += Time.deltaTime * meleeAttackSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 2;
            transform.position = Vector3.Lerp(startingAttackPosition, attackPosition, interpolation);

            yield return null;
        }

        //Storing the next time the attack can be done
        timeTillNextAttack = Time.time + timeBetweenAttack;

        //Activating the agent and stopping the attack
        attacking = false;
        agent.enabled = true;
    }

    protected IEnumerator Grab()
    {
        //Stopping the agent
        agent.enabled = false;

        //Getting the start position, direction to player and attack position
        Vector3 startingAttackPosition = transform.position;
        Vector3 dirToTarget = (player.position - transform.position).normalized;
        Vector3 attackPosition = player.position - dirToTarget;

        float percent = 0;
        bool hasGrabbed = false;

        while (percent <= 1)
        {
            //If grab has not been done while over 50% deal damage
            if (percent >= .5f && !hasGrabbed)
            {
                //Stopping the players agent and setting grabbed to true
                player.GetComponent<NavMeshAgent>().enabled = false;
                hasGrabbed = true;
            }

            //Moving the position the the attack position
            transform.position = Vector3.Lerp(startingAttackPosition, attackPosition, 2);

            percent += Time.deltaTime * grabTime;
            yield return null;

        }
        //Activating the players agent and enemy agent
        player.GetComponent<NavMeshAgent>().enabled = true;
        agent.enabled = true;

        //Storing the next grab time
        timeTillNextGrab = timeUntillGrab + Time.time;

    }

    protected IEnumerator Explosion()
    {
        //Waits for explosion time
        yield return new WaitForSeconds(timeUntilExplosion);

        //If player is within the explosion range then deals damage and removes enemy
        if (Vector3.Distance(player.position, transform.position) <= explosionRanage)
        {
            player.GetComponent<Entities>().TakeDamage(explosionDamage, "Explosion");
            Destroy(gameObject);
        }
    }

    // Shoots a single projectile and stops more from being shot until a set time has passed
    protected IEnumerator Shoot()
    {
        Projectile p = Instantiate(projectile, transform.position + Vector3.forward, Quaternion.identity);
        p.direction = new Vector3(player.position.x - transform.position.x, 0, player.position.z - transform.position.z).normalized;
        yield return new WaitForSeconds(fireRate);
        firing = null;
    }
}

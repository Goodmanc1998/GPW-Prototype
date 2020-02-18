using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PawnSpawnerScript : Entities
{

    //Storing the players Transform, and Agent for movement
    Transform player;
    NavMeshAgent agent;

    //Float to store distance that the player must be in to run away
    public float runAwayDistance;

    //Storing the pawn Enemy
    public PawnScript pawnEnemy;

    //floats used to store Times related for spawning
    public float timeBetweenSpawns;
    float timeTillNextSpawn;

    //Int to store max amount of pawns
    public int maxAmountToSpawn;

    int currentSpawned;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        
        if(player == null || agent == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            agent = GetComponent<NavMeshAgent>();
        }

        timeTillNextSpawn = Time.time + timeBetweenSpawns;

    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, player.position) < runAwayDistance)
        {
            MoveAwayFromPlayer();
            //Debug.Log("Running away");

        }

        if (Time.time >= timeTillNextSpawn & currentSpawned < maxAmountToSpawn)
        {
            SpawnPawn();
            //Debug.Log("Spawning");

        }
    }

    void SpawnPawn()
    {
        Vector3 spawnPosition = transform.position + (transform.forward * 2);

        Instantiate(pawnEnemy, spawnPosition, transform.rotation);

        pawnEnemy.pawnSpawner = this;

        currentSpawned++;

        timeTillNextSpawn = Time.time + timeBetweenSpawns;

    }

    public void PawnDied()
    {
        currentSpawned--;
    }

    void MoveAwayFromPlayer()
    {
        Vector3 dirToPlayer = Vector3.Normalize(transform.position - player.position);

        Vector3 movePosition = transform.position + dirToPlayer;

        agent.SetDestination(movePosition);

    }
}

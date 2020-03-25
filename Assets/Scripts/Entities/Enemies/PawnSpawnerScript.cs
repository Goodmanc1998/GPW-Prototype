﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PawnSpawnerScript : Enemy
{
    //Float to store distance that the player must be in to run away
    public float runAwayDistance;

    //Storing the pawn Enemy
    public PawnScript pawnEnemy;

    //floats used to store Times related for spawning
    public float timeBetweenSpawns;
    float timeTillNextSpawn;

    //Int to store max amount of pawns
    public int maxAmountToSpawn;

    public int currentSpawned;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        timeTillNextSpawn = Time.time + timeBetweenSpawns;

    }

    // Update is called once per frame
    void Update()
    {
        if(!froze)
        {
            if (Vector3.Distance(transform.position, player.position) < runAwayDistance)
            {
                SB("flee");

                //Debug.Log("Running away");

            }

            if (Time.time >= timeTillNextSpawn & currentSpawned < maxAmountToSpawn)
            {
                SpawnPawn();
                //Debug.Log("Spawning");

            }
        }
        

        if (dead)
        {
            Destroy(this.gameObject);
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

 
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PawnSpawnerScript : Enemy
{
    //Float to store distance that the player must be in to run away
    public float runAwayDistance;

    //Int to store max amount of pawns
    public int maxAmountToSpawn;
    private void Awake()
    {
        entitiesAnimator = gameObject.GetComponent<Animator>();
        entitiesAnimator.updateMode = UnityEngine.AnimatorUpdateMode.Normal; base.Start();

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



    public void PawnDied()
    {
        currentSpawned--;
    }

 
}

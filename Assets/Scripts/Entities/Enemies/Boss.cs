using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public float shootRange;
    public float disToPlayer;

    private int shot;
    protected override void Start()
    {
        startingHealth = 300;
        health = startingHealth;
        dead = false;
        shot = 0;
    }
    private void Update()
    {
        //fight starts
        
        if (!froze)
        {
            //health is > 33%
            if (health > health / 100 * 33)
            {
                float distance = Vector3.Distance(player.position, transform.position); // Calculates the distance between the player and enemy
                if (!froze)
                {
                    //keep distance to player 
                    // If the enemy is out of shooting range, move towards the player
                    if (distance > shootRange)
                    {
                        SB("persue");
                    }
                    // If the enemy gets within range of the player, shoot at them
                    else if (distance > disToPlayer)
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
            else if(health <= health/100*33)
            {
                //health is < 33%
                float distance = Vector3.Distance(player.position, transform.position); // Calculates the distance between the player and enemy
                //keep distance to player 
                // If the enemy is out of shooting range, move towards the player
                if (distance > shootRange)
                {
                    SB("persue");
                }
                // If the enemy gets within range of the player, shoot at them
                else if (distance > disToPlayer)
                {
                    //dashes in to melee attack flurry of 4 strikes
                    Shoot();
                    shot += 1;
                    // range attacks and special attacks in sequence
                    if (shot >= 4)
                    {
                        //after special attack froze in place take double damage.
                        SB("freeze");
                    }

                    SB("flee"); // Effectively stops the enemy from moving
                }
                
                
            }
        }
        
        

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    public float fleeRange; // Enemy will attempt to flee from the player when in this range
    public float shootRange; // Enemy will not fire at the player unless within this range
    public float fleeSpeed; // The speed the enemy will move whilst trying to get away from the player
    private void Awake()
    {
        entitiesAnimator = gameObject.GetComponent<Animator>();
        entitiesAnimator.updateMode = UnityEngine.AnimatorUpdateMode.Normal;
    }
    private void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position); // Calculates the distance between the player and enemy
        if(!froze)
        {
            // If the enemy is out of shooting range, move towards the player
            if (distance > shootRange)
            {
                SB("pursue");
            }
            // If the enemy gets within range of the player, shoot at them
            else if (distance > fleeRange)
            {
                enemyChosePosition = false;
                if (firing == null)
                {
                    firing = StartCoroutine(Shoot());
                    entitiesAnimator.SetTrigger("Attack");
                }
                SB("stop");
            }
            // If the player gets too close the enemy will stop shooting and attempt to run away from the player
            else
            {
                agent.Move((transform.position - player.position).normalized * fleeSpeed * Time.deltaTime);
            }

        }

        // Destroys the enemy when they die
        // Should this be done in the Entities class ??
        if (dead)
        {
            entitiesAnimator.SetTrigger("Damaged");
            Destroy(this.gameObject);
               
           
        }
    }


}

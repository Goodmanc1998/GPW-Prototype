using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    public float fleeRange; // Enemy will attempt to flee from the player when in this range
    public float shootRange; // Enemy will not fire at the player unless within this range
    public float fireRate; // The time in seconds between each shot projectile
    public float fleeSpeed; // The speed the enemy will move whilst trying to get away from the player

    public Projectile projectile; // The projectile that will be shot by this enemy

    Coroutine firing; // Keeps track of the shooting coroutine to ensure a delay between each shot

    private void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position); // Calculates the distance between the player and enemy

        // If the enemy is out of shooting range, move towards the player
        if (distance > shootRange)
        {
            agent.SetDestination(player.position);
        }
        // If the enemy gets within range of the player, shoot at them
        else if (distance > fleeRange)
        {
            if (firing == null)
            {
                firing = StartCoroutine(Shoot());
            }
            agent.SetDestination(transform.position); // Effectively stops the enemy from moving
        }
        // If the player gets too close the enemy will stop shooting and attempt to run away from the player
        else
        {
            agent.Move((transform.position - player.position).normalized * fleeSpeed * Time.deltaTime);
        }

        // Destroys the enemy when they die
        // Should this be done in the Entities class ??
        if (dead)
        {
            Destroy(this.gameObject);
        }
    }

    // Shoots a single projectile and stops more from being shot until a set time has passed
    IEnumerator Shoot()
    {
        Projectile p = Instantiate(projectile, transform.position + Vector3.forward, Quaternion.identity);
        p.direction = new Vector3(player.position.x - transform.position.x, 0, player.position.z - transform.position.z).normalized;
        yield return new WaitForSeconds(fireRate);
        firing = null;
    }
}

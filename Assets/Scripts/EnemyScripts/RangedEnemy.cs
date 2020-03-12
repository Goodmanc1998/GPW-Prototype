using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Entities
{
    public float fleeRange; // Enemy will attempt to flee from the player when in this range
    public float shootRange; // Enemy will not fire at the player unless within this range
    public float fireRate; // The time in seconds between each shot projectile
    public float fleeSpeed; // The speed the enemy will move whilst trying to get away from the player

    public Projectile projectile; // The projectile that will be shot by this enemy

    Coroutine firing;

    private void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        if (distance > shootRange)
        {
            agent.SetDestination(player.position);
        }
        else if (distance > fleeRange)
        {
            if (firing == null)
            {
                firing = StartCoroutine(Shoot());
            }
            agent.SetDestination(transform.position);
        }
        else
        {
            Debug.Log("Fleeing");
            agent.Move((transform.position - player.position).normalized * fleeSpeed * Time.deltaTime);
        }

        if (dead)
        {
            Destroy(this.gameObject);
        }
    }

    IEnumerator Shoot()
    {
        Projectile p = Instantiate(projectile, transform.position + Vector3.forward, Quaternion.identity);
        p.direction = new Vector3(player.position.x - transform.position.x, 0, player.position.z - transform.position.z).normalized;
        yield return new WaitForSeconds(fireRate);
        firing = null;
    }
}

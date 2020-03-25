using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingSpell : SpellMovement
{
    //Damage delt
    public int damage;
    //Total amount of enemys this can be chainned between
    public int chainAmount;
    //How far the chain can reach
    public int chainRange;
    //Current Chain amount
    int currentChain;

    //Next enemt to be chained to 
    GameObject nextEnemy;

    // Start is called before the first frame update
    void Start()
    {
        //Destroying the GameObject after its lifetime
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        //Calling the Movement function
        Movement();

        //If next enemy is available, starts moving towards this enemy
        if (nextEnemy != null)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, nextEnemy.transform.position, step);
        }
    }

    public override void Movement()
    {
        //Moving the transform forward 
        transform.position += (transform.forward * speed) * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider collision)
    {
        //Checking the collision is an enemy
        if (collision.gameObject.GetComponent<Entities>() == true && collision.gameObject.tag != "Player")
        {
            //Giving damage to the target
            Entities target = collision.gameObject.GetComponent<Entities>();
            target.TakeDamage(damage, "Lightning");

            currentChain++;

            //Checking if the current chain is less than the max chain amount
            if(currentChain < chainAmount)
            {
                //If it is then getting the next closest enemy
                nextEnemy = ClosestEnemy();
            }
            else
            {
                //Else this is destroyed
                Destroy(this.gameObject);
            }
        }
        else if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "Platform")
        {
            //If the collision is not an enemy it is removed
            Destroy(this.gameObject);
        }
    }

    GameObject ClosestEnemy()
    {
        //Getting colliders within range of current position
        Collider[] collidersWithinRange = Physics.OverlapSphere(transform.position, chainRange);

        //Getting current position
        Vector3 currentPos = transform.position;
        //Getting a min Distance
        float minDist = Mathf.Infinity;

        //used to store the closest Enemy
        GameObject closestEnemy = null;

        foreach (Collider c in collidersWithinRange)
        {
            //Checking the collider is an enemy
            if (c.gameObject.GetComponent<Entities>() && c.gameObject.tag != "Player")
            {
                //Getting the distance between the collider and current position
                float dist = Vector3.Distance(c.transform.position, currentPos);

                //Checking if the dist is less than the min distance
                if (dist < minDist && nextEnemy != c.gameObject)
                {
                    //Storing the new closest dist and enemy GameObject
                    closestEnemy = c.gameObject;
                    minDist = dist;
                }
            }

        }

        //Returning the closest Enemey GameObject
        return closestEnemy;
    }

    //Called just before Destroyed
    private void OnDestroy()
    {
        //Getting the player
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        //Updating the amount of current spells active
        player.GetComponent<PlayerMovement>().RemoveLightingSpell();
    }

}

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
    int currentChain = 0;

    //Amount of time
    public float stickEnemyTime;

    //Used to store the current hit enemy
    GameObject currentEnemy;
    //used to store the next enemy to move to
    GameObject nextEnemy;

    //A bool used to determine when the spell has hit an enemy
    bool hit;

    // Start is called before the first frame update
    void Start()
    {
        //Destroying the GameObject after its lifetime
        Destroy(this.gameObject, lifeTime);

    }

    // Update is called once per frame
    void Update()
    {
        //If there has not been a colision yet it will carry on forward
        if(hit != true)
        {
            //Moving the spell forward
            Movement();
        }

    }

    public override void Movement()
    {
        //Moving the transform forward 
        transform.position += (transform.forward * speed) * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //If there is a collision with an enemy
        if (collision.gameObject.GetComponent<Enemy>() == true)
        {
            //if the current chain is less than or equal to the chain amount
            if (currentChain < chainAmount)
            {
                //Setting hit to be true to prevent the spell moving forward like normal
                hit = true;

                //Storing the hit enemy
                currentEnemy = collision.gameObject;

                //If the damage is less than the health the spell will stick for a period of time
                if (damage < collision.gameObject.GetComponent<Entities>().health)
                {
                    //Starting the stick process
                    StartCoroutine(StickToEnemy(currentEnemy));

                    //Dealing Damage
                    collision.gameObject.GetComponent<Entities>().TakeDamage(damage, "Lightning");

                }//Else the spell will just deal damage and move to the next enemy
                else
                {
                    //Deal damage
                    collision.gameObject.GetComponent<Entities>().TakeDamage(damage, "Lightning");

                    //Starting to move to the next enemy
                    StartCoroutine(MoveToNextEnemy());

                }

                //Increasing the chain count
                currentChain++;
            }
            else
            {
                //if chain amount is greater then the spell is removed
                Destroy(this.gameObject);
            }

        }//if collision not with enemy or player
        else if (collision.gameObject.tag != "Player")
        {
            StartCoroutine(MoveToNextEnemy());
        }
    }

    IEnumerator StickToEnemy(GameObject enemy)
    {
        //Moving the transform with the current enemys transform
        transform.position = enemy.transform.position;

        //Waiting before moving on
        yield return new WaitForSeconds(stickEnemyTime);

        //Moving to the next enemy
        StartCoroutine(MoveToNextEnemy());

    }

    IEnumerator MoveToNextEnemy()
    {
        //Finding the closest enemy
        nextEnemy = ClosestEnemy();

        float percent = 0;

        //if closeset enemy is not null and chain is less than chain amount
        if(nextEnemy != null)
        {
            percent += Time.deltaTime * speed;

                //Moveing to the next enemy and waiting for this to happen
                transform.position = Vector3.Lerp(transform.position, nextEnemy.transform.position, percent);
                yield return null;

            
        }
        else
        {
            yield return null;
            //If no close enemy then destroy
            Destroy(this.gameObject);
        }
    }

    GameObject ClosestEnemy()
    {
        //Getting colliders within range of current position
        GameObject[] enemysWithinRange = GameObject.FindGameObjectsWithTag("Target");

        //Getting current position
        Vector3 currentPos = transform.position;
        //Getting a min Distance
        float minDist = Mathf.Infinity;

        //used to store the closest Enemy
        GameObject closestEnemy = null;

        foreach (GameObject g in enemysWithinRange)
        {
            //Checking the collider is an enemy
            if (g.gameObject.GetComponent<Enemy>() && Vector3.Distance(transform.position, g.transform.position) <= chainRange)
            {
                //Getting the distance between the collider and current position
                float dist = Vector3.Distance(g.transform.position, currentPos);

                //Checking if the dist is less than the min distance
                if (dist < minDist && currentEnemy != g.gameObject)
                {
                    //Storing the new closest dist and enemy GameObject
                    closestEnemy = g.gameObject;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpell : SpellMovement
{
    //int used to store damage delt
    public int damage;
    //int used to store AOE damage
    public int AOERange;
    //bool used to register when hit
    bool hit;
    //float used to store the time the gameObject has been alive for
    float timeAlive;

    public ParticleSystem fireBall;
    public ParticleSystem fireExplosion;


    // Start is called before the first frame update
    void Start()
    {
        //Destroying the GameObject after its lifetime
        Destroy(gameObject, lifeTime);

        //fireExplosion.sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1.0f, AOERange);
    }

    // Update is called once per frame
    void Update()
    {
        //If there has not been a colision yet it will carry on forward
        if (hit != true)
        {
            //Moving the spell forward
            Movement();
        }

        //Updating the time alive
        timeAlive += Time.deltaTime;
    }

    public override void Movement()
    {
        //Moving the transform forward 
        transform.position += (transform.forward * speed )* Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //If there is a collision with an enemy

        if (collision.gameObject.GetComponent<Enemy>() == true)
        {
            //Dealing damage to the hit enemy
            collision.gameObject.GetComponent<Entities>().TakeDamage(damage, "Fire");

            //Starting the fire AOE
            StartCoroutine(FireAOE());

        }//else if the collision is not with the player and the time alive is greater than 0.25
        else if (collision.gameObject.tag != "Player" && timeAlive > 0.25f)
        {
            //starting the fire AOE
            StartCoroutine(FireAOE());
        }

    }

    IEnumerator FireAOE()
    {
        //Setting hit to True
        hit = true;

        fireBall.Stop();

        fireExplosion.Play();

        //Storing colliders within ranage of the AOE Range
        Collider[] collidersWithinRange = Physics.OverlapSphere(transform.position, AOERange);

        //For each loop to run through each collider within array
        foreach (Collider c in collidersWithinRange)
        {
            //If the collider has the enemy script
            if (c.gameObject.GetComponent<Enemy>() == true)
            {
                //Dealing damage
                c.gameObject.GetComponent<Entities>().TakeDamage(damage, "Fire");
            }
        }

        //Waiting for length of fire aoe effect 
        yield return new WaitForSeconds(1);

        //Removing the gameObject
        Destroy(this.gameObject);
    }

    //Called just before Destroyed
    private void OnDestroy()
    {
        //Finding the player gameObject
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        //Updating the now current fire spell amount
        player.GetComponent<PlayerMovement>().RemoveFireSpell();
    }
    


}

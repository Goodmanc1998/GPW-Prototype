using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpell : SpellMovement
{
    public int damage;
    public int totalCollisions;
    int currentCollisions;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeTime);

    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    public override void Movement()
    {
        transform.position += (transform.forward * speed )* Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Entities>() == true && collision.gameObject.tag == "Target")
        {
            Debug.Log("Collision Enemy");

            Entities target = collision.gameObject.GetComponent<Entities>();

            target.TakeDamage(damage, "Fire");

            //FireDamage();

            currentCollisions++; 

            if(currentCollisions == totalCollisions)
            {
                Destroy(this.gameObject);

            }


        }
    }



}

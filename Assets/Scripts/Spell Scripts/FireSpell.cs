﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpell : SpellMovement
{
    public int damage;
    public float fireRange;

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
        GetComponent<Rigidbody>().transform.position += (GetComponent<Rigidbody>().transform.forward * speed )* Time.deltaTime;
    }

    public void FireDamage()
    {

        Collider[] collidersWithinRange = Physics.OverlapSphere(transform.position, fireRange);

        int count = 0;

        while (count < collidersWithinRange.Length)
        {
            if(collidersWithinRange[count].gameObject.GetComponent<Entities>())
            {
                collidersWithinRange[count].gameObject.GetComponent<EnemyScript>().TakeDamage(damage, "Fire");
            }

            count++;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Entities>() != null && collision.gameObject.tag != "Player")
        {
            Debug.Log("Collision Enemy");

            Entities target = collision.gameObject.GetComponent<Entities>();

            target.TakeDamage(damage, "Fire");

            //FireDamage();

            Destroy(gameObject);

        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpell : SpellMovement
{
    public int damage;
    public float fireRange;

    //public ParticleSystem fireEffect;


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
                collidersWithinRange[count].gameObject.GetComponent<EnemyScript>().applyDamage(damage);
            }

            count++;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<EnemyScript>() != null)
        {
            //collision.gameObject.GetComponent<EnemyScript>().Remove();
            Debug.Log("Collision Enemy");

            EnemyScript target = collision.gameObject.GetComponent<EnemyScript>();

            target.applyDamage(damage);

            FireDamage();

            Destroy(gameObject);

        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSpell : SpellMovement
{
    public float trapTime;

    private void Start()
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
        GetComponent<Rigidbody>().transform.position += (GetComponent<Rigidbody>().transform.forward * speed) * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<EnemyScript>() != null)
        {
            //collision.gameObject.GetComponent<EnemyScript>().Remove();
            Debug.Log("Collision Enemy");

            EnemyScript enemyScript = collision.gameObject.GetComponent<EnemyScript>();

            enemyScript.WaterTrap(trapTime);

            


            Destroy(gameObject);

        }
    }
}

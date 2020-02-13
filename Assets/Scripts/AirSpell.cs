using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirSpell : SpellMovement
{
    public int damage;
    public float PushRange;

    public int pushAmount;
    public float pushTime;



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
        GetComponent<Rigidbody>().transform.position += (GetComponent<Rigidbody>().transform.forward * speed) * Time.deltaTime;
    }

    public void PushBack()
    {

        Collider[] collidersWithinRange = Physics.OverlapSphere(transform.position, PushRange);

        //Debug.Log("Colliders in Range :" + collidersWithinRange.Length);

        int count = 0;

        while (count < collidersWithinRange.Length)
        {
            if (collidersWithinRange[count].gameObject.GetComponent<EnemyScript>())
            {
                collidersWithinRange[count].gameObject.GetComponent<EnemyScript>().PushBack(pushTime, pushAmount, transform.forward);
            }

            count++;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<EnemyScript>() != null)
        {
            //collision.gameObject.GetComponent<EnemyScript>().Remove();
            //Debug.Log("Collision Enemy");

            EnemyScript target = collision.gameObject.GetComponent<EnemyScript>();

            target.TakeDamage(damage, "Air");

            PushBack();

            Destroy(gameObject);

        }
    }
}

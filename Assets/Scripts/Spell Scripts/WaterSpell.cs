using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSpell : SpellMovement
{
    public int damage;
    public float trapTime;
    public float trapRange;

    public int pullAmount;



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

    public void PullIn()
    {

        Collider[] collidersWithinRange = Physics.OverlapSphere(transform.position, trapRange);

        int count = 0;

        while (count < collidersWithinRange.Length)
        {
            if (collidersWithinRange[count].gameObject.GetComponent<EnemyScript>())
            {
                collidersWithinRange[count].gameObject.GetComponent<EnemyScript>().PullIn(trapTime, pullAmount, (transform.position - collidersWithinRange[count].gameObject.transform.position));
            }

            count++;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<EnemyScript>() != null)
        {
            EnemyScript target = collision.gameObject.GetComponent<EnemyScript>();

            target.TakeDamage(damage, "Water");

            PullIn();

            Destroy(gameObject);

        }
    }
}

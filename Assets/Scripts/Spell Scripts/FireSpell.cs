using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpell : SpellMovement
{
    public int damage;
    public int AOERange;


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
        if (collision.gameObject.GetComponent<Entities>() == true && collision.gameObject.tag != "Player")
        {
            Entities target = collision.gameObject.GetComponent<Entities>();

            target.TakeDamage(damage, "Fire");

            FireAOE();

        }

    }

    public void FireAOE()
    {

        Collider[] collidersWithinRange = Physics.OverlapSphere(transform.position, AOERange);

        int count = 0;

        while (count < collidersWithinRange.Length)
        {
            if (collidersWithinRange[count].gameObject.GetComponent<Entities>() && collidersWithinRange[count].gameObject.tag != "Player")
            {
                collidersWithinRange[count].gameObject.GetComponent<Entities>().TakeDamage(damage, "Fire");
            }

            count++;
        }

        Destroy(this.gameObject);

    }

    private void OnDestroy()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        player.GetComponent<PlayerMovement>().RemoveFireSpell();
    }


}

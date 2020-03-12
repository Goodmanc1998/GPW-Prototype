using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingSpell : SpellMovement
{
    public int damage;
    public int chainAmount;
    public int chainRange;
    int currentChain;


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
        transform.position += (transform.forward * speed) * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.GetComponent<Entities>() == true && collision.gameObject.tag != "Player")
        {
            Entities target = collision.gameObject.GetComponent<Entities>();

            target.TakeDamage(damage, "Lightning");

            if(currentChain < chainAmount)
            {
                LookAtNewEnemy();
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void LookAtNewEnemy()
    {
        Collider[] collidersWithinRange = Physics.OverlapSphere(transform.position, chainRange);

        int count = 0;

        while (count < collidersWithinRange.Length)
        {
            if (collidersWithinRange[count].gameObject.GetComponent<Entities>() && collidersWithinRange[count].gameObject.tag != "Player")
            {
                if(Vector3.Distance(collidersWithinRange[count].transform.position, transform.position) > 0.5)
                {
                    Vector3 lookDir = collidersWithinRange[count].transform.position - transform.position;

                    Quaternion lookRotation = Quaternion.LookRotation(lookDir);

                    transform.rotation = lookRotation;

                    currentChain++;
                }
            }

            count++;
        }
    }

    private void OnDestroy()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        player.GetComponent<PlayerMovement>().RemoveLightingSpell();
    }

}

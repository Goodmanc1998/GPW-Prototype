using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class castedSpellSpeed : MonoBehaviour
{

    public int damage;

    public bool fireSpell;

    public int speed;
    
    void Update()
    {
        GetComponent<Rigidbody>().transform.position += (GetComponent<Rigidbody>().transform.forward * speed) * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    { 
        if(collision.gameObject.GetComponent<EnemyScript>() !=  null)
        {
            //collision.gameObject.GetComponent<EnemyScript>().Remove();
            Debug.Log("Collision Enemy");

            EnemyScript target = collision.gameObject.GetComponent<EnemyScript>();

            target.applyDamage(damage);

            Destroy(gameObject);

        }
    }
}

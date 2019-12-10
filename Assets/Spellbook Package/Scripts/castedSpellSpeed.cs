using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class castedSpellSpeed : MonoBehaviour
{
    
    void Update()
    {
        GetComponent<Rigidbody>().transform.position += GetComponent<Rigidbody>().transform.forward * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<EnemyScript>() !=  null)
        {
            collision.gameObject.GetComponent<EnemyScript>().Remove();
            Debug.Log("Collision Enemy");
        }
    }
}

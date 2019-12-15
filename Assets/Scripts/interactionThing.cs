using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactionThing : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Platform")
        {
            other.gameObject.GetComponent<Rigidbody>().AddExplosionForce(3000,transform.position, 100);
        }
    }
}

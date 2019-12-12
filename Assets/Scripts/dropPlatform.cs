using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dropPlatform : MonoBehaviour
{
    private int timer = 0;
    private bool playerOn= false;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {

            playerOn = true;
            
            
        }
    }
    private void Update()
    {
        if(playerOn)
        {
            timer++;
            if(timer >= 30) //roughly half a second
            {
                gameObject.GetComponent<Rigidbody>().useGravity = true;
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }
}

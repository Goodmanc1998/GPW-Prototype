using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellMovement : MonoBehaviour
{
    public int speed;
    public float lifeTime;

    public virtual void Movement()
    {
        GetComponent<Rigidbody>().transform.position += GetComponent<Rigidbody>().transform.forward * Time.deltaTime;
    }

}

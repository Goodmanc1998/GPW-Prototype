using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellMovement : MonoBehaviour
{
    public int speed;
    public float lifeTime;

    public virtual void Movement()
    {
        transform.position += (transform.forward * speed) * Time.deltaTime;
    }

}

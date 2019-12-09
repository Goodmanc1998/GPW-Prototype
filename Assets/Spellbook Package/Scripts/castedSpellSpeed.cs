using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class castedSpellSpeed : MonoBehaviour
{
    
    void Update()
    {
        GetComponent<Rigidbody>().transform.position += GetComponent<Rigidbody>().transform.forward * Time.deltaTime;
    }
}

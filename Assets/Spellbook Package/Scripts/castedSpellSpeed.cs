using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class castedSpellSpeed : MonoBehaviour
{
    
    void Update()
    {
        GetComponent<Rigidbody>().transform.position += Vector3.one * Time.deltaTime;
    }
}

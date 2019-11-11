using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyerSimple : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}

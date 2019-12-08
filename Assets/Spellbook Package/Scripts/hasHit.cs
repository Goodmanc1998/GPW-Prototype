using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hasHit : MonoBehaviour 
{
    [SerializeField]
    public bool _hasHit = false;

    private void OnTriggerEnter(Collider other)
    {
        _hasHit = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BridgeAnimation : MonoBehaviour
{

    public Animator bridgeAnimator;

    public GameObject block;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            block.GetComponent<NavMeshObstacle>().enabled = true;

            if(bridgeAnimator != null)
            {
                bridgeAnimator.SetBool("colapse", true);

            }
        }
    }
}

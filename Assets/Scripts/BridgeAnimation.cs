using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BridgeAnimation : MonoBehaviour
{
    public Animator bridgeAnimator;

    public GameObject block;

    public AudioClip bridgeCollapseSound;
    AudioSource source;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            block.GetComponent<NavMeshObstacle>().enabled = true;

            if(bridgeAnimator != null)
            {
                source = gameObject.AddComponent<AudioSource>();
                source.clip = bridgeCollapseSound;
                source.volume = 0.8f;
                source.Play();
                bridgeAnimator.SetBool("colapse", true);
            }
        }
    }
}

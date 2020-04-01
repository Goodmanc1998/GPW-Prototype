using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DoorAnimation : MonoBehaviour
{

    public static string playerTag = "Player";

    Animator animator;


    // Start is called before the first frame update
    void Start()
    {
            animator = GetComponent<Animator>();
            animator.SetBool("Dooropen", false);

    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.tag == playerTag)
        {
            animator.SetBool("Dooropen", true);

        }
    }
    private void OnTriggerExit(Collider c)
    {
        if (c.tag == playerTag)
        {
            animator.SetBool("Dooropen", false);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

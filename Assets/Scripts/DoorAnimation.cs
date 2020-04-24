using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DoorAnimation : MonoBehaviour
{
    public static string playerTag = "Player";

    public AudioClip openDoorSound;
    public AudioClip closeDoorSound;

    AudioSource source;

    Animator animator;

    TutorialScript tutorial;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("Dooropen", false);

        tutorial = GameObject.FindGameObjectWithTag("Tutorial").GetComponent<TutorialScript>();

        source = gameObject.AddComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.tag == playerTag && tutorial.tutorialComplete)
        {
            source.clip = openDoorSound;
            source.volume = 0.8f;
            source.Play();
            animator.SetBool("Dooropen", true);
        }
    }
    private void OnTriggerExit(Collider c)
    {
        if (c.tag == playerTag && tutorial.tutorialComplete)
        {
            source.clip = closeDoorSound;
            source.volume = 0.8f;
            source.Play();
            animator.SetBool("Dooropen", false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

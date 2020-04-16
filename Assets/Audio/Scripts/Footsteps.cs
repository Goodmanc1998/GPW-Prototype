using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Footsteps : MonoBehaviour
{
    public float speedBeforePlayingSound; // The players velocity required before footsteps are played
    public SoundEffect[] footstepSounds; // Array of possible footstep sounds

    AudioSource source; // The audio source attached to the player used for playing footstep sounds
    bool playFootsteps = true; // Uses to determine if footstep sound effects should be playing

    NavMeshAgent player; // Uses to check if the player is moving

    private void Start()
    {
        // Locate the NavMeshAgent and AudioSource on the player object
        player = gameObject.GetComponent<NavMeshAgent>();
        source = gameObject.AddComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        Ray ray = new Ray(transform.position, Vector3.down); // Raycast should be directed below the player
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 2f))
        {
            //Debug.Log(hit.transform.gameObject.name + " @ " + hit.transform.tag + " | " + playFootsteps);
            SetFootstepSound(hit.transform.gameObject.tag.ToLower()); // Set the footstep sound according to the players walking surface
        }
    }

    private void Update()
    {
        // Footsteps should be playing if the player is moving
        if (player.velocity.magnitude > speedBeforePlayingSound)
        {
            playFootsteps = true;
        }
        else
        {
            playFootsteps = false;
        }

        // If the audio source is stopped and footsteps should be playing then play the audio source
        if (!source.isPlaying && playFootsteps)
        {
            source.Play();
        }
        // If the audio source is playing and footsteps should not be played then pause the audio source
        else if (source.isPlaying && !playFootsteps)
        {
            source.Pause();
        }
    }

    // Searches the array of sounds and sets the audio sources to use the correct clip
    void SetFootstepSound(string name)
    {
        for (int i = 0; i < footstepSounds.Length; i++)
        {
            // Surface tag should be same as the corresponding sound effect name
            if (footstepSounds[i].name == name)
            {
                source.clip = footstepSounds[i].sound;
                source.volume = footstepSounds[i].volume;
            }
        }
    }
}

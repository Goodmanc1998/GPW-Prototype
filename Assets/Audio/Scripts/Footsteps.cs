using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Footsteps : MonoBehaviour
{
    public float speedBeforePlayingSound; // The players velocity required before footsteps are played
    public FootstepArray[] footstepSounds; // Array of sounds by their surface, each surface contains an array of possible sounds

    FootstepArray currentSoundArray; // Players current possible footstep sounds based on where they are walking

    AudioSource source; // The audio source attached to the player used for playing footstep sounds
    bool playFootsteps = true; // Uses to determine if footstep sound effects should be playing

    NavMeshAgent player; // Used to check if the player is moving

    private void Start()
    {
        // Locate the NavMeshAgent and AudioSource on the player object
        player = gameObject.GetComponent<NavMeshAgent>();
        source = gameObject.AddComponent<AudioSource>();

        foreach (FootstepArray array in footstepSounds)
        {
            array.InitArray();
        }
    }

    private void FixedUpdate()
    {
        Ray ray = new Ray(transform.position, Vector3.down); // Raycast should be directed below the player
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 2f))
        {
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

        // If the audio source is stopped and footsteps should be playing then choose a footstep sound and play the audio source
        if (!source.isPlaying && playFootsteps && currentSoundArray != null)
        {
            if (currentSoundArray.soundArray.Length > 0)
            {
                SetRandomSound(currentSoundArray);
                source.Play();
            }
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
            if (footstepSounds[i].surfaceName == name)
            {
                currentSoundArray = footstepSounds[i];
            }
        }
    }

    // Chooses a weighted random sound from the surfaces possible footstep sounds
    void SetRandomSound(FootstepArray sounds)
    {
        int random = Random.Range(0, sounds.totalWeighting);
        for (int i = 0, current = 0; i < sounds.soundArray.Length; i++)
        {
            current += sounds.soundArray[i].weight;
            if (random < current)
            {
                source.clip = sounds.soundArray[i].sound;
                source.volume = sounds.soundArray[i].volume;
                return;
            }
        }
    }
}

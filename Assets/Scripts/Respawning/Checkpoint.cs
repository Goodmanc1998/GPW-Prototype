using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Checkpoint : MonoBehaviour
{
    public int checkpointNumber; // The number of this checkpoint; a lower number means the checkpoint is triggered sooner

    static int currentCheckpoint = -1; // The current checkpoint the player will respawn at

    private void Start()
    {
        if (!GetComponent<Collider>().isTrigger)
        {
            Debug.LogWarning("Checkpoint colliders must be triggers. Setting Collider.isTrigger to true");
            GetComponent<Collider>().isTrigger = true;
        }
    }

    // When the player comes into contact with the attached collider, check to see if their spawnpoint should be set
    private void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Player"))
        {
            // If the current checkpoint is further back than this one, set this checkpoint as the players checkpoint
            if (currentCheckpoint < checkpointNumber)
            {
                PlayerMovement player = c.GetComponent<PlayerMovement>();

                player.SetCheckpoint(this); // Set the players checkpoint
                currentCheckpoint = checkpointNumber;
            }
        }
    }
}

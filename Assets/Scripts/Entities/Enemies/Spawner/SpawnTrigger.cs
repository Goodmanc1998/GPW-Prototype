using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SpawnTrigger : MonoBehaviour
{
    public static string playerTag = "Player";

    EnemySpawnManager manager;
    bool started = false;

    private void Start()
    {
        manager = (EnemySpawnManager)FindObjectOfType(typeof(EnemySpawnManager));
        if (!GetComponent<Collider>().isTrigger)
        {
            Debug.LogWarning("AreaStart collider must be of type Trigger. Setting isTrigger to true.");
            GetComponent<Collider>().isTrigger = true;
        }
    }

    // When the player is in range, the wave will begin
    private void OnTriggerEnter(Collider c)
    {
        if (c.tag == playerTag && !started)
        {
            started = true;
            manager.StartWave(this);
        }
    }

    private void Reset()
    {
        if (gameObject.layer != 2)
        {
            Debug.LogWarning("Area triggers layer must be set to IgnoreRaycast");
            gameObject.layer = 2; // Sets the collider to ignore raycasts
        }
    }
}

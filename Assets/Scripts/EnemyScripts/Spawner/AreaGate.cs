using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AreaGate : MonoBehaviour
{
    public int waveNumber = 1; // The wave number that will open this upon completion

    EnemySpawnManager manager;

    // Subscribes the open method to the specified wave
    void Start()
    {
        manager = (EnemySpawnManager)FindObjectOfType(typeof(EnemySpawnManager));
        if (waveNumber <= 0 || waveNumber > manager.waves.Count)
        {
            Debug.LogWarning("Wave number out of range.");
            return;
        }
        manager.waves[waveNumber-1].EndWave += OpenArea;
    }

    // Abstract method, write a new monobehaviour for each gate using the OpenArea() method to handle individual opening code
    public abstract void OpenArea();
}

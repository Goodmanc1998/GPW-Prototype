using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AreaGate : MonoBehaviour
{
    public int waveNumber; // The wave number that will open this upon completion

    EnemySpawnManager manager;

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

    public abstract void OpenArea();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AreaGate : MonoBehaviour
{
    public int[] waveNumbers; // The wave number that will open this upon completion
    int completedWaves;

    EnemySpawnManager manager;

    // Subscribes the open method to the specified wave
    void Start()
    {
        manager = (EnemySpawnManager)FindObjectOfType(typeof(EnemySpawnManager));
      
        completedWaves = waveNumbers.Length;
        for (int i = 0; i < waveNumbers.Length; i++)
        {
            if (waveNumbers[i] <= 0 || waveNumbers[i] > manager.waves.Count)
            {
                Debug.LogWarning("Wave number out of range.");
                return;
            }
            manager.waves[waveNumbers[i] - 1].EndWave += CompletedWave;
        }
    }

    // Notes that a wave has been completed and checks to see if OpenArea() should be called
    public void CompletedWave()
    {
        Debug.Log("Completed a wave: " + gameObject.name + " | " + completedWaves + " to go.");
        completedWaves--;
        if (completedWaves <= 0)
        {
            OpenArea();
        }
    }

    // Abstract method, write a new monobehaviour for each gate using the OpenArea() method to handle individual opening code
    public abstract void OpenArea();
   
}

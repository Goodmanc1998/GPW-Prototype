using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public List<EnemySpawn> spawnpoints; // The list of possible spawn locations for the enemies
    public EnemyScript enemy; // Array of enemies that can be spawned in this wave
    public int enemyAmount; // The total number of enemies that are spawned throughout the wave
    public float difficulty; // Can be used to balance enemy health, strength etc. individually for each wave
    public float waveStartTime; // The time after the start wave method is called before enemies start spawning
    public float averageSpawningDuration; // The length of time enemies are spawned for, a time of 0 will spawn all enemies instantly
    public float spawnDurationRandomness; // The amount of irregularity of enemy spawns, a time of 0 will make each enemy spawn in regular intervals

    public int enemiesRemaining; // When no more enemies are remaining the end wave method is called

    // Editor variables, do not affect gameplay
    public Color spawnpointColour; // Colour of the spawnpoint gizmos for the wave
    public bool showSpawnpoints = false; // Shows the spawnpoint dropdown list where each position can be changed manually

    public delegate void WaveAction();
    public event WaveAction EndWave;

    public Wave()
    {
        enemyAmount = 10;
        difficulty = 1;
        waveStartTime = 0;
        averageSpawningDuration = 5;
        spawnDurationRandomness = 0;
        showSpawnpoints = false;
    }

    // Calculates the time before the next enemy is spawned
    public float SpawnRate()
    {
        return Random.Range(averageSpawningDuration - spawnDurationRandomness, averageSpawningDuration + spawnDurationRandomness) / enemyAmount;
    }

    public void EnemyKilled()
    {
        enemiesRemaining--;
        if (enemiesRemaining <= 0)
        {
            EndWave();
        }
    }
}
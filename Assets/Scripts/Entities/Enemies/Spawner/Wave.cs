using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public List<EnemySpawn> spawnpoints; // The list of possible spawn locations for the enemies
    public Enemy enemy; // Enemy that will be spawned in this wave
    public SpawnTrigger startTrigger; // Trigger collider that will start the wave once the player has entered it
    public int enemyAmount; // The total number of enemies that are spawned throughout the wave
    public float averageSpawningDuration; // The length of time enemies are spawned for, a time of 0 will spawn all enemies instantly
    public float spawnDurationRandomness; // The amount of irregularity of enemy spawns, a time of 0 will make each enemy spawn in regular intervals

    public int enemiesRemaining = 0; // When no more enemies are remaining the end wave method is called
    public bool started = false;

    // Editor variables, do not affect gameplay
    public Color spawnpointColour; // Colour of the spawnpoint gizmos for the wave
    public bool showSpawnpoints = false; // Shows the spawnpoint dropdown list where each position can be changed manually

    public delegate void WaveAction();
    public event WaveAction EndWave;

    public Wave()
    {
        enemyAmount = 10;
        //difficulty = 1;
        averageSpawningDuration = 5;
        spawnDurationRandomness = 0;
        showSpawnpoints = false;
    }

    // Calculates the time before the next enemy should be spawned
    public float SpawnRate()
    {
        return Random.Range(averageSpawningDuration - spawnDurationRandomness, averageSpawningDuration + spawnDurationRandomness) / enemyAmount;
    }

    public void EnemyKilled()
    {
        enemiesRemaining--;
        if (enemiesRemaining <= 0)
        {
            Debug.Log("Wave eliminated: " + enemy.name);
            EndWave();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public int enemyAmount;
    public float difficulty;
    public float waveStartTime;
    public float averageSpawningDuration;
    public float spawnDurationRandomness;

    public float SpawnRate()
    {
        return Random.Range(averageSpawningDuration - spawnDurationRandomness, averageSpawningDuration + spawnDurationRandomness) / enemyAmount;
    }

    public Wave()
    {
        enemyAmount = 10;
        difficulty = 1;
        waveStartTime = 0;
        averageSpawningDuration = 5;
        spawnDurationRandomness = 0;
    }
}

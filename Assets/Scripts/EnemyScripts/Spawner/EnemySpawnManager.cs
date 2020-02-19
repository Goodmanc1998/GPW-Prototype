﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnManager : MonoBehaviour
{
    public List<Wave> waves; // List of waves currently in use

    public bool blockOnScreenSpawns; // When a spawn location is within this range of the player, no enemies will be spawned from it
    
    // Currently has no use
    public int enemySpawnLimit; // The maximum amount of enemies across all waves allowed to be present in the world, set to 0 for no limit (only applies to wave spawned enemies)

    /*
    IEnumerator StartSpawning()
    {
        for (int i = 0; i < waves.Count; i++)
        {
            StartCoroutine(SpawnWave(waves[i]));
            if (i < waves.Count - 1) yield return new WaitForSeconds(waves[i + 1].waveStartTime - waves[i].waveStartTime);
        }
    }
    */

    public void StartWave(int waveNumber)
    {
        StartCoroutine(SpawnWave(waves[waveNumber]));
    }

    IEnumerator SpawnWave(Wave wave)
    {
        if (wave.enemy == null)
        {
            Debug.LogWarning("Wave has no attached enemy object. Wave cancelled.");
            yield break;
        }
        wave.enemiesRemaining = wave.enemyAmount;
        int totalSpawned = 0;
        while (totalSpawned < wave.enemyAmount)
        {
            if (SpawnEnemy(wave))
            {
                totalSpawned++;
            }
            yield return new WaitForSeconds(wave.SpawnRate());
        }
    }

    public bool SpawnEnemy(Wave wave)
    {
        List<EnemySpawn> possibleSpawns = new List<EnemySpawn>();
        if (blockOnScreenSpawns)
        {
            foreach (EnemySpawn spawnpoint in wave.spawnpoints)
            {
                Vector3 p = Camera.main.WorldToViewportPoint(spawnpoint.position);
                if (!(p.x > 0 && p.x < 1 && p.y > 0 && p.y < 1 && p.z > 0))
                {
                    possibleSpawns.Add(spawnpoint);
                }
            }
        }
        else
        {
            possibleSpawns = wave.spawnpoints;
        }
        if (possibleSpawns.Count == 0)
        {
            Debug.LogWarning("No spawnpoints found. Spawn aborted.");
            return false;
        }
        Entities spawnedEnemy = Instantiate(wave.enemy, possibleSpawns[Random.Range(0, possibleSpawns.Count)].position, Quaternion.identity);
        spawnedEnemy.wave = wave;
        spawnedEnemy.startingHealth = (int)(spawnedEnemy.startingHealth * wave.difficulty);
        return true;
    }

    private void OnDrawGizmos()
    {
        Color original = Gizmos.color;
        for (int i = 0; i < waves.Count; i++)
        {
            Gizmos.color = waves[i].spawnpointColour;
            if (waves[i].spawnpoints != null)
            {
                for (int j = 0; j < waves[i].spawnpoints.Count; j++)
                {
                    Vector3 pos = waves[i].spawnpoints[j].position;
                    Gizmos.DrawWireSphere(pos, 0.025f * Vector3.Distance(pos, UnityEditor.SceneView.lastActiveSceneView.camera.transform.position));
                }
            }
        }
        Gizmos.color = original;
    }
}

[System.Serializable]
public class EnemySpawn
{
    public Vector3 position;

    public EnemySpawn(Vector3 position)
    {
        this.position = position;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnManager : MonoBehaviour
{
    public static int enemyCount = 0;

    public List<EnemySpawn> spawnpoints; // The list of possible spawn locations for the enemies
    public List<Wave> waves;

    public bool showSpawnpoints = false;

    public Transform player;
    public EnemyScript enemy;

    public bool blockOnScreenSpawns; // When a spawn location is within this range of the player, no enemies will be spawned from it
    public int enemySpawnLimit; // The maximum amount of enemies allowed to be present in the world, set to -1 for no limit

    System.Random randomGen = new System.Random();

    private void Start()
    {
        /*
        if (player == null)
        {
            Debug.LogWarning("Player cannot be set to null, enemy spawning disabled");
        }
        */
        {
            StartCoroutine(StartSpawning());
        }
    }

    IEnumerator StartSpawning()
    {
        for (int i = 0; i < waves.Count; i++)
        {
            StartCoroutine(SpawnWave(waves[i]));
            yield return new WaitForSeconds(waves[i + 1].waveStartTime - waves[i].waveStartTime);
        }
        //Debug.Log("Waves finished spawning");
    }

    IEnumerator SpawnWave(Wave wave)
    {
        int totalSpawned = 0;
        while (totalSpawned < wave.enemyAmount)
        {
            //Debug.Log("Attempting spawn");
            if (SpawnEnemy(wave.difficulty))
            {
                //Debug.Log("Spawn successful");
                totalSpawned++;
            }
            yield return new WaitForSeconds(wave.SpawnRate());
        }
    }

    public bool SpawnEnemy(float difficulty)
    {
        if (enemyCount >= enemySpawnLimit && enemySpawnLimit >= 0)
        {
            return false;
        }
        List<EnemySpawn> possibleSpawns = new List<EnemySpawn>();
        if (blockOnScreenSpawns)
        {
            foreach (EnemySpawn spawnpoint in spawnpoints)
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
            possibleSpawns = spawnpoints;
        }
        if (possibleSpawns.Count == 0)
        {
            Debug.Log("No spawnpoints found. Spawn aborted.");
            return false;
        }
        EnemyScript spawnedEnemy = Instantiate(enemy, possibleSpawns[Random.Range(0, possibleSpawns.Count)].position, Quaternion.identity);
        spawnedEnemy.startingHealth = (int)(spawnedEnemy.startingHealth * difficulty);
        spawnedEnemy.timeBetweenAttacks /= difficulty;
        return true;
    }

    private void OnDrawGizmos()
    {
        Color original = Gizmos.color;
        Gizmos.color = Color.cyan;
        foreach (EnemySpawn spawnpoint in spawnpoints)
        {
            Gizmos.DrawWireSphere(spawnpoint.position, 2);
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
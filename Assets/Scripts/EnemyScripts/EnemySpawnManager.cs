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

    public float playerBlockSpawnRange; // When a spawn location is within this range of the player, no enemies will be spawned from it
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
            if (SpawnEnemy())
            {
                //Debug.Log("Spawn successful");
                totalSpawned++;
            }
            yield return new WaitForSeconds(wave.SpawnRate());
        }
    }

    public bool SpawnEnemy()
    {
        if (enemyCount >= enemySpawnLimit && enemySpawnLimit >= 0)
        {
            return false;
        }
        List<EnemySpawn> possibleSpawns = new List<EnemySpawn>();
        foreach (EnemySpawn spawnpoint in spawnpoints)
        {
            //if (Vector3.Distance(spawnpoint.position, player.position) > playerBlockSpawnRange)
            {
                possibleSpawns.Add(spawnpoint);
            }
        }
        Instantiate(enemy, possibleSpawns[Random.Range(0, possibleSpawns.Count)].position, Quaternion.identity);
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
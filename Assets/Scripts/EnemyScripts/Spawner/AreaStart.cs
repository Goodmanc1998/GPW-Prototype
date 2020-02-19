﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AreaStart : MonoBehaviour
{
    public static string playerTag = "Player";

    public int waveNumber = 1; // The wave number that will be started when the player reaches this collider

    EnemySpawnManager manager;

    private void Start()
    {
        manager = (EnemySpawnManager)FindObjectOfType(typeof(EnemySpawnManager));
        if (!GetComponent<Collider>().isTrigger)
        {
            Debug.LogWarning("AreaStart collider must be of type Trigger. Setting isTrigger to true.");
            GetComponent<Collider>().isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.tag == playerTag)
        {
            manager.StartWave(waveNumber - 1);
            Debug.Log("Started Wave");
        }
    }
}

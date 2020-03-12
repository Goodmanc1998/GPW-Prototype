using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Entities : MonoBehaviour
{

    //Storing the players Transform, and Agent for movement
    protected Transform player;
    protected NavMeshAgent agent;

    

    public Wave wave; // The wave the enemy was spawned in

    [Header("Health")]

    public float startingHealth;
    public float health;
    protected bool dead;

    [Header("Movemnt")]


    public float movementSpeed;
    public float angularSpeed;
    public float acceleration;
    public float stoppingDistance;

    public enum weaknessEnum
    {
        Nothing,
        Fire,
        Lightning,
        Melee
    };

    public weaknessEnum weakness;

    protected virtual void Start()
    {
        health = startingHealth;

        if (player == null || agent == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            agent = GetComponent<NavMeshAgent>();
        }

        agent.speed = movementSpeed;
        agent.angularSpeed = angularSpeed;
        agent.acceleration = acceleration;
        agent.stoppingDistance = stoppingDistance;

    }

    public virtual void TakeDamage(float damageIn, string attackType)
    {

        if(damageIn >= health)
        {
            health -= damageIn;

            dead = true;



            // Once the enemy is declared dead, tell the wave an enemy has died if there is one
            if (wave != null)
            {
                //wave.EnemyKilled();
            }


        }
        else
        {

            if(weakness == weaknessEnum.Nothing || attackType != weakness.ToString())
            {
                health -= damageIn;
            }
            else
            {
                health -= damageIn * 1.5f;
            }
        }
    }

}

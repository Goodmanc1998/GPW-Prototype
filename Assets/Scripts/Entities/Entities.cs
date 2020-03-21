using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Entities : MonoBehaviour
{
    //Storing the players Transform, and Agent for movement
    protected Transform player;
    protected NavMeshAgent agent;

    [Header("Health")]
    public float startingHealth;
    public float health;
    protected bool dead;

    public enum WeaknessEnum
    {
        Nothing,
        Fire,
        Lightning,
        Melee
    };

    public WeaknessEnum weakness;

    protected virtual void Start()
    {
        health = startingHealth;

        if (player == null || agent == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            agent = GetComponent<NavMeshAgent>();
        }
    }

    public virtual void TakeDamage(float damageIn, string attackType)
    {

        if(damageIn >= health)
        {
            health -= damageIn;

            dead = true;
        }
        else
        {

            if(weakness == WeaknessEnum.Nothing || attackType != weakness.ToString())
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

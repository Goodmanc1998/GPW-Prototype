using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Entities : MonoBehaviour
{
    //Storing the players Transform, and Agent for movement
    protected Transform player;
    protected NavMeshAgent agent;
    public Animator entitiesAnimator;

    public AudioClip deathSound;
    public AudioClip takeDamageSound;

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
            entitiesAnimator = GetComponent<Animator>();
            player = GameObject.FindGameObjectWithTag("Player").transform;
            agent = GetComponent<NavMeshAgent>();
        }
    }

    public virtual void TakeDamage(float damageIn, string attackType)
    {
        if(damageIn >= health && dead == false)
        {
            health -= damageIn;
            if (deathSound != null) AudioSource.PlayClipAtPoint(deathSound, transform.position);
            dead = true;
        }
        else
        {
            if (takeDamageSound != null) AudioSource.PlayClipAtPoint(takeDamageSound, transform.position);
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

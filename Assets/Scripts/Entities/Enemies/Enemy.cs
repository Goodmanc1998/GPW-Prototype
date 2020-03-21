using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entities
{
    [Header("Movement")]
    public float movementSpeed;
    public float angularSpeed;
    public float acceleration;
    public float stoppingDistance;

    [HideInInspector]
    public Wave wave; // The wave the enemy was spawned in

    protected override void Start()
    {
        base.Start();

        agent.speed = movementSpeed;
        agent.angularSpeed = angularSpeed;
        agent.acceleration = acceleration;
        agent.stoppingDistance = stoppingDistance;
    }

    public override void TakeDamage(float damageIn, string attackType)
    {
        base.TakeDamage(damageIn, attackType);

        // Once the enemy is declared dead, tell the wave an enemy has died if there is one
        if (wave != null)
        {
            wave.EnemyKilled();
        }
    }

    //steering behaviours.
    public virtual void SB(string calledFor)
    {
        if (calledFor != "")
        {
            //so steeringbehaviours have a call made by one of the AI to make a move what move is it?
            //seek
            //flee
            //persue
            //avoid
        }
    }
}

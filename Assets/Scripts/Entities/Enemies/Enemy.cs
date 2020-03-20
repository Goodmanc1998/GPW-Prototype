using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entities
{
    public Wave wave; // The wave the enemy was spawned in

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

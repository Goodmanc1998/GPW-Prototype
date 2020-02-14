using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entities : MonoBehaviour
{
    public Wave wave; // The wave the enemy was spawned in

    public float startingHealth;
    public float health;
    protected bool dead;

    public enum weaknessEnum
    {
        Nothing,
        Fire,
        Earth,
        Wind,
        Water,
        Melee
    };

    public weaknessEnum weakness;

    protected virtual void Start()
    {
        health = startingHealth;
        
    }

    public virtual void TakeDamage(float damageIn, string attackType)
    {

        if(damageIn >= health)
        {
            dead = true;

            // Once the enemy is declared dead, tell the wave an enemy has died if there is one
            if (wave != null)
            {
                wave.EnemyKilled();
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

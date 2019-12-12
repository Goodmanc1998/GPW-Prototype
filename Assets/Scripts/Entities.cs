using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entities : MonoBehaviour
{
    int health;

    public virtual void applyDamage(int dmgToDo)
    {
        //applies a damage to current health;

        if (health <= dmgToDo)
        {
            Destroy(gameObject);
        }
        else
        {
            health -= dmgToDo;
        }
    }

    public int applyHeal(int healAmount, int currentHealth)
    {
        //applies a heal to current health and returns the value.
        var newHealth = currentHealth + healAmount;
        return newHealth;
    }

}

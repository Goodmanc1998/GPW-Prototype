using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entities : MonoBehaviour
{
    public int health;
    public float speed;
    public Rigidbody entityRgbdy;

    public int applyDamage(int dmgToDo, int currentHealth)
    {
        //applies a damage to current health and returns the value;
         var newHealth = currentHealth - dmgToDo;
        return newHealth;
    }

    public int applyHeal(int healAmount, int currentHealth)
    {
        //applies a heal to current health and returns the value.
        var newHealth = currentHealth + healAmount;
        return newHealth;
    }

    public virtual Vector3 applyMovement(Vector3 currentPos, Vector3 dir, float speed)
    {
        // implement code here.
        Vector3 newPos = currentPos + dir * speed;
        return newPos;
    }
}

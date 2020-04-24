using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage; // How much damage will be done to the player
    public float timeBeforeDestroyed; // How long before the projectile is destroyed in the event it dosen't hit anything
    [HideInInspector]
    public Vector3 direction; // The direction of the projectile
    [HideInInspector]
    public float speed; // The speed of the projectile

    float rotateSpeed = 15f; // Makes the projectile spin in midair

    float age;

    private void Awake()
    {
        age = timeBeforeDestroyed;
    }

    // Moves the projectile and checks to see if it should be destroyed
    private void Update()
    {
        transform.Rotate(Random.onUnitSphere * rotateSpeed);
        transform.position += direction * speed * Time.deltaTime;
        age -= Time.deltaTime;
        if (age <= 0)
        {
            Destroy(gameObject);
        }
    }

    // If the projectile hits something other than another enemy, destroy it and deal damage to the player if it hits them
    private void OnTriggerEnter(Collider collider)
    {
        string tag = collider.gameObject.tag;
        if (tag == "Player" && tag != "Boss") // If it hits the player they take damage
        {
            collider.gameObject.GetComponent<PlayerMovement>().TakeDamage(damage, "None");
            Destroy(gameObject);
        }
        if (tag == "Enviroment" || tag == "Stone" || tag == "Dirt" || tag == "Wood" && tag != "Boss") // Pass through other enemies
        {
            Destroy(gameObject); // Destroy the projectile when it hits something other than an enemy
        }
    }
}

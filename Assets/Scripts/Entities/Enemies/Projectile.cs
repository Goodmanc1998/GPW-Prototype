using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed; // The speed of the projectile
    public float damage; // How much damage will be done to the player
    public float timeBeforeDestroyed; // How long before the projectile is destroyed in the event it dosen't hit anything
    [HideInInspector]
    public Vector3 direction;

    float age;

    private void Awake()
    {
        age = timeBeforeDestroyed;
    }

    // Moves the projectile and checks to see if it should be destroyed
    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        age -= Time.deltaTime;
        if (age <= 0)
        {
            Destroy(gameObject);
        }
    }

    // If the projectile hits something, destroy it and deal damage to the player if it hits them
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovement>())
        {
            collision.gameObject.GetComponent<PlayerMovement>().TakeDamage(damage, "None");
        }
        Destroy(gameObject);
    }
}

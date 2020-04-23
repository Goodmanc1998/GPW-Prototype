using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpell : SpellMovement
{

    public float damage;

    public float rotationSpeed;
    bool follow;

    Rigidbody rb;

    Transform playerT;

    float timeAlive;


    // Start is called before the first frame update
    void Start()
    {
        playerT = GameObject.FindGameObjectWithTag("Player").transform;

        rb = GetComponent<Rigidbody>();

        Destroy(this.gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        timeAlive += Time.deltaTime;

        if (timeAlive > 0.25f)
            follow = true;

        if (!follow)
            Movement();

        if (follow)
            FollowPlayer();

    }

    void FollowPlayer()
    {
        Vector3 direction = (playerT.position - transform.position).normalized;

        float step = rotationSpeed * Time.deltaTime;

        Vector3 newDir = Vector3.RotateTowards(transform.forward, direction, step, 0.0f);

        transform.rotation = Quaternion.LookRotation(newDir);

        Movement();

    }

    private void OnCollisionEnter(Collision collision)
    {

        Debug.Log(collision.gameObject.name);

        if(collision.gameObject.GetComponent<PlayerMovement>() == true)
        {
            collision.gameObject.GetComponent<PlayerMovement>().TakeDamage(damage, "none");

            Destroy(gameObject);

        }
        else if(collision.gameObject.tag != "Boss" && !collision.gameObject.GetComponent<BossSpell>())
        {
            Destroy(gameObject);
        }

    }
}

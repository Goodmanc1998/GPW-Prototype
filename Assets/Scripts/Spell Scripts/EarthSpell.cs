using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthSpell : SpellMovement
{
    public float distanceForWallSpawn;
    public GameObject wall;

    Transform player;

    bool created = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        if(Vector3.Distance(player.position, transform.position) > distanceForWallSpawn &&  created == false)
        {
            CreateWall();
            created = true;
        }
    }

    public override void Movement()
    {
        GetComponent<Rigidbody>().transform.position += (GetComponent<Rigidbody>().transform.forward * speed) * Time.deltaTime;
    }

    public void CreateWall()
    {
        Destroy(Instantiate(wall, transform.position, Quaternion.identity), lifeTime);

        Destroy(gameObject);
    }
}

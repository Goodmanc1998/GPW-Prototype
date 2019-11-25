using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entities
{
    public Vector3 veolcity;
    public Vector3 axisSpeed;
    public int jumpCount = 0;
    /*Parent method calls...
    applyDamage
    applyHeal
    applyMovement can be overrided to implment custom movement for the player.
         */

    public Vector3 dir;

    public GameObject grapple;
    public GameObject grappleSpawn;


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Platform")
        {
            jumpCount = 0; 
            
        }
    }
    public override Vector3 applyMovement(Vector3 currentVel, Vector3 dir, float speed)
    {

        currentVel = new Vector3(dir.x, entityRgbdy.velocity.y, dir.z);
        //Vector3 newVelocity = currentVel * speed * Time.deltaTime;
        Vector3 newVelocity = new Vector3(currentVel.x * speed * Time.deltaTime, currentVel.y, currentVel.z * speed * Time.deltaTime);

        entityRgbdy.transform.rotation = Quaternion.LookRotation(-dir *Time.deltaTime, Vector3.up);
        return newVelocity;
    }

    private void applyJump(Vector3 currentPos, Vector3 dir, float jumpForce)
    {
        
        entityRgbdy.AddExplosionForce(jumpForce, currentPos, 10);

    }// same as movement but handles only jump and is purely for the player.
    void Start()
    {
        health = 100;
        speed = 15f;
        entityRgbdy = GetComponent<Rigidbody>();
    }
    void Update()
    {

        Vector3 camOffset = new Vector3(0f, 3, -7.5f);
        FindObjectOfType<Camera>().transform.position = transform.position + camOffset;
        transform.position = entityRgbdy.position;
        //transform.rotation = entityRgbdy.rotation;
        transform.rotation = Quaternion.LookRotation(-dir * Time.deltaTime, Vector3.up);

        /*
        
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            if(jumpCount == 0)
            {
                axisSpeed = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
                entityRgbdy.velocity = new Vector3(applyMovement(entityRgbdy.velocity, new Vector3(axisSpeed.x * speed, 0, axisSpeed.z * speed), speed).x, entityRgbdy.velocity.y, applyMovement(entityRgbdy.velocity, new Vector3(axisSpeed.x * speed, 0, axisSpeed.z * speed), speed).z);
            }
            else
            {
                axisSpeed = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")) /1.4f;
                entityRgbdy.velocity = new Vector3(applyMovement(entityRgbdy.velocity, new Vector3(axisSpeed.x * speed, 0, axisSpeed.z * speed), speed).x, entityRgbdy.velocity.y, applyMovement(entityRgbdy.velocity, new Vector3(axisSpeed.x * speed, 0, axisSpeed.z * speed), speed).z);
            }
            
        }
        else { }
        */

        
        if (Input.GetKey(KeyCode.W))
        {
           entityRgbdy.velocity =  applyMovement(entityRgbdy.velocity, Vector3.forward, speed);
            
        }
        if(Input.GetKey(KeyCode.S))
        {
            entityRgbdy.velocity = applyMovement(entityRgbdy.velocity, -Vector3.forward, speed);
           
        }
        if (Input.GetKey(KeyCode.A))
        {
            entityRgbdy.velocity = applyMovement(entityRgbdy.velocity, -Vector3.right, speed);
            
        }
        if (Input.GetKey(KeyCode.D))
        {
            entityRgbdy.velocity = applyMovement(entityRgbdy.velocity, Vector3.right, speed);
            
        }
        if (Input.GetButtonDown("Jump") && jumpCount < 2)
        {
            jumpCount += 1;
            applyJump(new Vector3(entityRgbdy.position.x , entityRgbdy.position.y, entityRgbdy.position.z), Vector3.up, 666f);
            
        }
        

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(grapple, grappleSpawn.transform.position, grappleSpawn.transform.rotation);
        }
        
    }

    public void GetDirection(Vector3 distanceDirection, Vector3 dir_)
    {
        Vector3 newDirection = new Vector3(distanceDirection.x, 0, distanceDirection.y);
        entityRgbdy.velocity = applyMovement(entityRgbdy.velocity, newDirection, speed);

        dir = new Vector3(dir_.x, 0, dir_.y);
        
    }

    public void playerJump()
    {
        if(jumpCount < 2)
        {
            jumpCount++;
            applyJump(new Vector3(entityRgbdy.position.x, entityRgbdy.position.y, entityRgbdy.position.z), Vector3.up, 666f);

        }
    }

    public void grappleHit(Vector3 hitPoint)
    {
        transform.position = Vector3.MoveTowards(transform.position, hitPoint, 10f);
    }
}

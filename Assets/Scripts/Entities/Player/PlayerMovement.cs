﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : Entities
{
    Camera viewCamera;

    Checkpoint checkpoint;

    GameObject targetEnemy;
    public float targetRange;

    float timebetweenChecks = 0.5f;
    float nextCheckTime;

    private int speedBuffCounter = 0;

    //spells effect prefabs
    public Transform speedBuff;

    public GameObject FireSpell;
    public int maxFireSpell;
    int currentFireSpell;

    public GameObject LightningSpell;
    public int maxLightingSpell;
    int currentLightingSpell;

    public GameObject groundSpell;
    public Transform spellSpawn;

    public GameObject healthBar;
    Vector3 hitPoint;
    public void castSpell(LineRenderer GestureTransform,string ShapeDrawn,float percentMatch)
    {
        if(percentMatch >= 0.9f)
        {
            if(ShapeDrawn == "line" && percentMatch >=0.95f && currentFireSpell < maxFireSpell)
            {
                //cast circle spell here by instatiating spell object.
                //Debug.Log("Circle");
                //transform.LookAt(GestureTransform.GetPosition(GestureTransform.positionCount-1));

                transform.LookAt(new Vector3(GestureTransform.GetPosition(GestureTransform.positionCount - 1).x, transform.position.y, GestureTransform.GetPosition(GestureTransform.positionCount - 1).z));

                entitiesAnimator.SetBool("SpellDrawn", true);
                Instantiate(FireSpell, spellSpawn.position, transform.localRotation);

                Debug.Log(transform.localRotation);
                Debug.Log(GestureTransform.GetPosition(GestureTransform.positionCount - 1));

                currentFireSpell++;


            }
            else if (ShapeDrawn == "lightening" && currentLightingSpell < maxFireSpell)
            {
                //cast triangle spell here by instatiating spell object.
                //Debug.Log("triangle");
                transform.LookAt(new Vector3(GestureTransform.GetPosition(GestureTransform.positionCount - 1).x, transform.position.y, GestureTransform.GetPosition(GestureTransform.positionCount - 1).z));
                entitiesAnimator.SetBool("SpellDrawn", true);
                Instantiate(LightningSpell, spellSpawn.position, transform.localRotation);
                Debug.Log("cast lightening");

                currentLightingSpell++;
            }
            else if (ShapeDrawn == "arc")
            {
                //cast square spell here by instatiating spell object.
                //Debug.Log("square");
                //depth for rotating the wall correctly
                float minusOneMid = GestureTransform.GetPosition((GestureTransform.positionCount / 2) - 1).z;
                float plusOneMid = GestureTransform.GetPosition((GestureTransform.positionCount / 2) + 1).z;
                Debug.Log(GestureTransform.positionCount);
                entitiesAnimator.SetBool("SpellDrawn", true);
                for (int i = 0; i < GestureTransform.positionCount; i++)
                {
                    
                    Instantiate(groundSpell, GestureTransform.GetPosition(i), Quaternion.Euler(0, 90, 0));
                }
                //deprecated was original process of spawn the ice wall but then moved onto the approach.!!
                //LEFT here incase we could make use of this for any other reasons.
                //if(minusOneMid < plusOneMid)
                //{
                //    Instantiate(groundSpell, GestureTransform.GetPosition(GestureTransform.positionCount / 2), Quaternion.Euler(0,90,0));
                //    Debug.Log("cast here and the angle is still wrong");
                //}else if (minusOneMid > plusOneMid)
                //{
                //    Instantiate(groundSpell, GestureTransform.GetPosition(GestureTransform.positionCount / 2), Quaternion.Euler(0, 0, 0));
                //    Debug.Log("cast here and the angle is still wrong");
                //}
                
                
            }
            
        }
        else
        {
            entitiesAnimator.SetTrigger("SpellFail");
            //spell was a failure do little fizzle out of a spell to indicate to player they were close to casting.
        }
        
    }


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        entitiesAnimator.updateMode = UnityEngine.AnimatorUpdateMode.Normal;
        viewCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(entitiesAnimator.GetBool("SpellDrawn") == true)
        {
            for (int i = 0; i < 120; i++)
            {
                entitiesAnimator.SetBool("SpellDrawn", false);
            }
        }
        if(Vector3.Distance(hitPoint,transform.position) <= 0.5f)
        {
            entitiesAnimator.SetBool("PlayerMove", false);
        }
        healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(health, 100);

        if(speedBuffCounter >= 1)
        {
            speedBuffCounter++;
            if(speedBuffCounter >= 300)
            {
                agent.speed -= 5;
                speedBuffCounter = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && !Input.GetKey(KeyCode.Q))
        {

            Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {

                hitPoint = hit.point;

                if(hit.collider.gameObject.tag == "Platform" || hit.collider.gameObject.tag == "Enviroment")
                {
                    agent.SetDestination(hitPoint);
                    entitiesAnimator.SetBool("PlayerMove", true);
                }

            }
        }

        CheckTarget();

        if (targetEnemy != null && Time.time > nextCheckTime)
        {
            PlayerLook();
            nextCheckTime = Time.time + timebetweenChecks;
        }
        if (dead)
        {
            entitiesAnimator.SetBool("Dead", true);
            Respawn(); // If the player dies, respawn them
        }
    }

    public void PlayerLook()
    {
        Vector3 lookDir = targetEnemy.transform.position - transform.position;

        Quaternion lookRotation = Quaternion.LookRotation(lookDir);

        Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * agent.angularSpeed).eulerAngles;

        transform.rotation = Quaternion.Euler(0.0f, rotation.y, 0.0f);
    }

    public void CheckTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Target");

        GameObject nearestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {

            float targetDistance = Vector3.Distance(transform.position, enemy.transform.position);

            if(targetDistance <= closestDistance)
            {
                closestDistance = targetDistance;
                nearestEnemy = enemy;
            }

        }

        if(nearestEnemy != null && closestDistance < targetRange)
        {
            targetEnemy = nearestEnemy;
        }
        else
        {
            targetEnemy = null;
        }
    }

    public void RemoveFireSpell()
    {
        currentFireSpell--;
    }

    public void RemoveLightingSpell()
    {
        currentLightingSpell--;
    }

    // Set the location the player should be spawned at when they die
    public void SetCheckpoint(Checkpoint newCheckpoint)
    {
        Debug.Log("Set checkpoint");
        checkpoint = newCheckpoint;
    }

    // Respawn the player
    public void Respawn()
    {
        transform.position = checkpoint.transform.position; // Set the player to the checkpoints location
        health = startingHealth; // Reset the players health
        EnemySpawnManager.ResetWaves(); // Reset all waves that are currently active or have yet to fought
    }
}
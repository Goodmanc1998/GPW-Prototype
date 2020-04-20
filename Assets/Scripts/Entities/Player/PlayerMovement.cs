using System.Collections;
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

    TutorialScript tutorial;

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

                //Debug.Log(transform.localRotation);
                //Debug.Log(GestureTransform.GetPosition(GestureTransform.positionCount - 1));

                currentFireSpell++;

                if (tutorial.spellOneCast == false)
                    tutorial.spellOneCast = true;


            }
            else if (ShapeDrawn == "lightening" && currentLightingSpell < maxFireSpell)
            {
                //cast triangle spell here by instatiating spell object.
                //Debug.Log("triangle");
                transform.LookAt(new Vector3(GestureTransform.GetPosition(GestureTransform.positionCount - 1).x, transform.position.y, GestureTransform.GetPosition(GestureTransform.positionCount - 1).z));
                entitiesAnimator.SetBool("SpellDrawn", true);
                Instantiate(LightningSpell, spellSpawn.position, transform.localRotation);
                //Debug.Log("cast lightening");

                currentLightingSpell++;

                if (tutorial.spellTwoCast == false)
                    tutorial.spellTwoCast = true;
            }
            else if (ShapeDrawn == "arc")
            {
                //cast square spell here by instatiating spell object.
                //Debug.Log("square");
                //depth for rotating the wall correctly
                float minusOneMid = GestureTransform.GetPosition((GestureTransform.positionCount / 2) - 1).z;
                float plusOneMid = GestureTransform.GetPosition((GestureTransform.positionCount / 2) + 1).z;
                //Debug.Log(GestureTransform.positionCount);
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

                if (tutorial.spellThreeCast == false)
                    tutorial.spellThreeCast = true;

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

        tutorial = GameObject.FindGameObjectWithTag("Tutorial").GetComponent<TutorialScript>();
    }

    // Update is called once per frame
    void Update()
    {

// Allows us to speed up the game in the unity editor for quick testing
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.KeypadMultiply))
        {
            Time.timeScale++;
        }
        if (Input.GetKey(KeyCode.KeypadDivide))
        {
            Time.timeScale--;
        }
#endif

        if (entitiesAnimator.GetBool("SpellDrawn") == true)
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


        if(speedBuffCounter >= 1)
        {
            speedBuffCounter++;
            if(speedBuffCounter >= 300)
            {
                agent.speed -= 5;
                speedBuffCounter = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && !Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.LeftAlt))
        {

            Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                hitPoint = hit.point;

                string tag = hit.collider.gameObject.tag;
                if(tag == "Platform" || tag == "Enviroment" || tag == "Dirt" || tag == "Stone" || tag == "Wood")
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
            StartCoroutine(Respawn(4f)); // If the player dies, respawn them
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
        checkpoint = newCheckpoint;
    }

    // Respawn the player
    public IEnumerator Respawn(float timeBeforeRespawn)
    {
        Debug.Log("Player Respawning");
        yield return new WaitForSeconds(timeBeforeRespawn); // Allow time for the death animation to play
        entitiesAnimator.SetBool("Dead", false); // Reset the animators death state
        transform.position = checkpoint.transform.position; // Set the player to the checkpoints location
        health = startingHealth; // Reset the players health
        viewCamera.GetComponent<CameraFollow>().ResetCamera(); // Reset the camera position
        EnemySpawnManager.ResetWaves(); // Reset all waves that are currently active or have yet to fought
    }
}
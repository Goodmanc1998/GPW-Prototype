using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : Entities
{
    Camera viewCamera;

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
            //spell was a failure do little fizzle out of a spell to indicate to player they were close to casting.
        }
    }


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        viewCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

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

                Vector3 hitPoint = hit.point;

                if(hit.collider.gameObject.tag == "Platform")
                {
                    agent.SetDestination(hitPoint);
                }

            }
        }

        CheckTarget();

        if (targetEnemy != null && Time.time > nextCheckTime)
        {
            PlayerLook();
            nextCheckTime = Time.time + timebetweenChecks;
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


}
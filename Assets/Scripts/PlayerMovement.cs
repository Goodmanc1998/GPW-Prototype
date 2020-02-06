using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    Camera viewCamera;
    public NavMeshAgent player;

    public Rigidbody playerRigidbody;

    public LayerMask target;
    public GameObject targetEnemy;
    public float targetRange;

    private int speedBuffCounter = 0;

    //spells effect prefabs
    public Transform speedBuff;

    public GameObject FireSpell;
    public GameObject AirSpell;
    public GameObject waterSpell;
    public GameObject groundSpell;
    public Transform spellSpawn;

    public void castSpell(string ShapeDrawn,float percentMatch)
    {
        if(percentMatch >= 0.8f)
        {
            if(ShapeDrawn == "circle")
            {
                //cast circle spell here by instatiating spell object.
                //Debug.Log("Circle");

                Instantiate(waterSpell, spellSpawn.position, spellSpawn.rotation);

            }else if (ShapeDrawn == "triangle")
            {
                //cast triangle spell here by instatiating spell object.
                //Debug.Log("triangle");
                Instantiate(AirSpell, spellSpawn.position, spellSpawn.rotation);
            }
            else if (ShapeDrawn == "square")
            {
                //cast square spell here by instatiating spell object.
                //Debug.Log("square");
                Instantiate(groundSpell, spellSpawn.position, spellSpawn.rotation);
            }
            else if (ShapeDrawn == "skull")
            {
                //cast skull spell here by instatiating spell object.
                Debug.Log("skull");
                Instantiate(FireSpell, spellSpawn.position, spellSpawn.rotation);

            }
            else if (ShapeDrawn == "lightning")
            {
                //cast lightning spell here by instatiating spell object.
                Debug.Log("lightning");
                Instantiate(speedBuff,gameObject.transform.position, Quaternion.identity);
                player.speed += 5;
                speedBuffCounter += 1;
            }
        }
        else
        {
            //spell was a failure do little fizzle out of a spell to indicate to player they were close to casting.
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        viewCamera = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {
        
        if(speedBuffCounter >= 1)
        {
            speedBuffCounter++;
            if(speedBuffCounter >= 300)
            {
                player.speed -= 5;
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

                if(hit.collider.gameObject.name == "Terrain")
                {
                    player.SetDestination(hitPoint);
                }

            }
        }

        CheckTarget();

        if (targetEnemy != null)
        {
            PlayerLook();
        }


    }

    public void PlayerLook()
    {
        Vector3 lookDir = targetEnemy.transform.position - transform.position;

        Quaternion lookRotation = Quaternion.LookRotation(lookDir);

        Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * player.angularSpeed).eulerAngles;

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


}
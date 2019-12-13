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
                if (hit.collider.gameObject.tag == "Target")
                {
                    if (hit.collider.gameObject == targetEnemy)
                    {
                        targetEnemy = null;
                    }
                    else
                    {
                        targetEnemy = hit.collider.gameObject;
                    }


                }else if (hit.collider.gameObject.tag == "SpellBook")
                {

                }
                else
                {
                    player.SetDestination(hitPoint);
                }
            }
        }

        if (targetEnemy != null)
        {
            Vector3 newLook = new Vector3(targetEnemy.transform.position.x, 0, targetEnemy.transform.position.z);

            playerRigidbody.transform.LookAt(newLook);
        }


    }


}
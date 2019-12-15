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

    

    public float rotSpeed;
    public void castSpell(string ShapeDrawn,float percentMatch)
    {
        if(percentMatch >= 0.9f)
        {
            if(ShapeDrawn == "Circle")
            {
                //cast circle spell here by instatiating spell object.
                Debug.Log("Circle");
            }else if (ShapeDrawn == "triangle")
            {
                //cast triangle spell here by instatiating spell object.
                Debug.Log("triangle");
            }
            else if (ShapeDrawn == "Square")
            {
                //cast square spell here by instatiating spell object.
                Debug.Log("Square");
            }
            else if (ShapeDrawn == "Air")
            {
                //cast skull spell here by instatiating spell object.
                Debug.Log("Air");
            }
            else if (ShapeDrawn == "Penta")
            {
                
                //cast lightning spell here by instatiating spell object.
                Debug.Log("Penta");
                int count = 0;
                if (count == 0)
                {
                    _ = Instantiate(speedBuff, gameObject.transform.position, Quaternion.identity) as Transform;
                    count = 1;
                    if(count >= 1)
                    {
                        
                            FindObjectOfType<DrawToScreen>().clearPoint();
                        
                        
                    }
                    
                }
                
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

            playerRigidbody.transform.LookAt(targetEnemy.transform);
        }


    }


}
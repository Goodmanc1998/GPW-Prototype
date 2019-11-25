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

    public float rotSpeed;

    // Start is called before the first frame update
    void Start()
    {
        viewCamera = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {

            Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {

                Vector3 hitPoint = hit.point;
                if(hit.collider.gameObject.tag == "Target")
                {
                    if(hit.collider.gameObject == targetEnemy)
                    {
                        targetEnemy = null;
                    }
                    else
                    {
                        targetEnemy = hit.collider.gameObject;
                    }


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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    public Camera viewCamera;
    public NavMeshAgent player;

    public Rigidbody playerRigidbody;

    public LayerMask target;
    public GameObject targetEnemy;

    EarthSpell earthSpell;

    [Header("SpellBook Stuff")]
    public GameObject spellCasted; /*
    public GameObject SquareSpell;
    public GameObject CircleSpell;
    public GameObject TraingleSpell;
    public GameObject XSpell; */

    public GameObject spawnPoint;

    /*
    [Header("square spell collider array")]
    public hasHit[] sqaureColliders;
    [Header("circle spell collider array")]
    public hasHit[] circleColliders;
    [Header("traingle spell collider array")]
    public hasHit[] traingleColliders;
    [Header("X spell collider array")]
    public hasHit[] xColliders;

    public float rotSpeed; */

    // Start is called before the first frame update
    void Start()
    {
        /*
        //viewCamera = Camera.main;
        sqaureColliders = new hasHit[12];
        circleColliders = new hasHit[12];
        traingleColliders = new hasHit[12];
        xColliders = new hasHit[12];
        var ss = SquareSpell.GetComponentsInChildren<hasHit>();
        var cs = CircleSpell.GetComponentsInChildren<hasHit>();
        var ts = TraingleSpell.GetComponentsInChildren<hasHit>();
        var xs = XSpell.GetComponentsInChildren<hasHit>();
        for (int i = 0; i < 12; i++)
        {
            sqaureColliders[i] = ss[i+i];
            circleColliders[i] = cs[i + i];
            traingleColliders[i] = ts[i + i];
            xColliders[i] = xs[i + i];

        } */

        earthSpell = GetComponent<EarthSpell>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            Destroy(Instantiate(spellCasted, spawnPoint.transform.position, spawnPoint.transform.rotation), 5);
            //earthSpell.CreateWall();

        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
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
            Vector3 newLook = new Vector3(targetEnemy.transform.position.x, transform.position.y, targetEnemy.transform.position.z);

            playerRigidbody.transform.LookAt(newLook);
        }


    }


}
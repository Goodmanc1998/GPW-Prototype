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

    [Header("SpellBook Stuff")]
    public GameObject spellCasted;
    public GameObject SquareSpell;
    public GameObject CircleSpell;
    public GameObject TraingleSpell;
    public GameObject XSpell;

    [Header("square spell collider array")]
    public hasHit[] sqaureColliders;
    [Header("circle spell collider array")]
    public hasHit[] circleColliders;
    [Header("traingle spell collider array")]
    public hasHit[] traingleColliders;
    [Header("X spell collider array")]
    public hasHit[] xColliders;

    public float rotSpeed;

    // Start is called before the first frame update
    void Start()
    {
        viewCamera = Camera.main;
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

        } 
    }

    // Update is called once per frame
    void Update()
    {
        if(sqaureColliders[0]._hasHit == true)
        {
            if (sqaureColliders[1]._hasHit == true)
            {
                if (sqaureColliders[2]._hasHit == true)
                {
                    if (sqaureColliders[3]._hasHit == true)
                    {
                        if (sqaureColliders[4]._hasHit == true)
                        {
                            if (sqaureColliders[5]._hasHit == true)
                            {
                                if (sqaureColliders[6]._hasHit == true)
                                {
                                    if (sqaureColliders[7]._hasHit == true)
                                    {
                                        if (sqaureColliders[8]._hasHit == true)
                                        {
                                            if (sqaureColliders[9]._hasHit == true)
                                            {
                                                if (sqaureColliders[10]._hasHit == true)
                                                {
                                                    if (sqaureColliders[11]._hasHit == true)
                                                    {
                                                        Debug.Log("Big ass spell time wooooohooooo");
                                                        Instantiate(spellCasted, transform.position, transform.rotation);
                                                        for (int i = 0; i < 12; i++)
                                                        {

                                                            sqaureColliders[i]._hasHit = false;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        if (circleColliders[0]._hasHit == true)
        {
            if (circleColliders[1]._hasHit == true)
            {
                if (circleColliders[2]._hasHit == true)
                {
                    if (circleColliders[3]._hasHit == true)
                    {
                        if (circleColliders[4]._hasHit == true)
                        {
                            if (circleColliders[5]._hasHit == true)
                            {
                                if (circleColliders[6]._hasHit == true)
                                {
                                    if (circleColliders[7]._hasHit == true)
                                    {
                                        if (circleColliders[8]._hasHit == true)
                                        {
                                            if (circleColliders[9]._hasHit == true)
                                            {
                                                if (circleColliders[10]._hasHit == true)
                                                {
                                                    if (circleColliders[11]._hasHit == true)
                                                    {
                                                        Debug.Log("Big ass spell time wooooohooooo");
                                                        Instantiate(spellCasted, transform.position, transform.rotation);
                                                        for (int i = 0; i < 12; i++)
                                                        {
                                                            circleColliders[i]._hasHit = false;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        if (traingleColliders[0]._hasHit == true)
        {
            if (traingleColliders[1]._hasHit == true)
            {
                if (traingleColliders[2]._hasHit == true)
                {
                    if (traingleColliders[3]._hasHit == true)
                    {
                        if (traingleColliders[4]._hasHit == true)
                        {
                            if (traingleColliders[5]._hasHit == true)
                            {
                                if (traingleColliders[6]._hasHit == true)
                                {
                                    if (traingleColliders[7]._hasHit == true)
                                    {
                                        if (traingleColliders[8]._hasHit == true)
                                        {
                                            if (traingleColliders[9]._hasHit == true)
                                            {
                                                if (traingleColliders[10]._hasHit == true)
                                                {
                                                    if (traingleColliders[11]._hasHit == true)
                                                    {
                                                        Debug.Log("Big ass spell time wooooohooooo");
                                                        Instantiate(spellCasted, transform.position, transform.rotation);
                                                        for (int i = 0; i < 12; i++)
                                                        {
                                                            traingleColliders[i]._hasHit = false;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        if (xColliders[0]._hasHit == true)
        {
            if (xColliders[1]._hasHit == true)
            {
                if (xColliders[2]._hasHit == true)
                {
                    if (xColliders[3]._hasHit == true)
                    {
                        if (xColliders[4]._hasHit == true)
                        {
                            if (xColliders[5]._hasHit == true)
                            {
                                if (xColliders[6]._hasHit == true)
                                {
                                    if (xColliders[7]._hasHit == true)
                                    {
                                        if (xColliders[8]._hasHit == true)
                                        {
                                            if (xColliders[9]._hasHit == true)
                                            {
                                                if (xColliders[10]._hasHit == true)
                                                {
                                                    if (xColliders[11]._hasHit == true)
                                                    {
                                                        Debug.Log("Big ass spell time wooooohooooo");
                                                        Instantiate(spellCasted, transform.position, transform.rotation);
                                                        for (int i = 0; i < 12; i++)
                                                        {
                                                            xColliders[i]._hasHit = false;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
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
            Vector3 newLook = new Vector3(targetEnemy.transform.position.x, 0, targetEnemy.transform.position.z);

            playerRigidbody.transform.LookAt(targetEnemy.transform);
        }


    }


}
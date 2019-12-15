using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rope : MonoBehaviour
{
    public Vector3 target;
    public GameObject joint;
    GameObject Player;
    GameObject lastJoint;
    public bool complete = false;
    // Start is called before the first frame update
    void Start()
    {
        Player = FindObjectOfType<Player>().gameObject;
        lastJoint = transform.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, 0.5f);
        if(transform.position != target)
        {
            if(Vector3.Distance(Player.transform.position, lastJoint.transform.position) >= 0.5f)
            {
                createJoint();
            }
        }else if(complete == false)
        {
            complete = true;
            lastJoint.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            lastJoint.GetComponent<HingeJoint>().connectedBody = Player.GetComponent<Rigidbody>();
        }
    }

    void createJoint()
    {
        Vector3 pointCreat = Player.transform.position - lastJoint.transform.position;
        pointCreat.Normalize();
        pointCreat *= 0.5f;
        pointCreat += lastJoint.transform.position;

        GameObject currJoint = (GameObject)Instantiate(joint, pointCreat, Quaternion.identity);
        lastJoint.GetComponent<HingeJoint>().connectedBody = currJoint.GetComponent<Rigidbody>();
        lastJoint = currJoint;
        currJoint.transform.SetParent(lastJoint.transform);

    }
}

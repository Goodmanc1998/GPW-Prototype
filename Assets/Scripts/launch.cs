using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class launch : MonoBehaviour
{
    public GameObject grapple;
    private GameObject currGrapple;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) 
            )
        {
            var Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 camOffset = new Vector3(0, 0, 7.5f);
            Vector3 target = Ray.GetPoint(camOffset.z);
           
            currGrapple = (GameObject)Instantiate(grapple, transform.position, Quaternion.identity);
            currGrapple.GetComponent<rope>().target = target;
        }
    }
}

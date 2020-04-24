
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrazierPosition : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // sends the transform position to the Coruption shader using Global vector
        Shader.SetGlobalVector("_Brazier1Position", transform.position);
    }
   
}
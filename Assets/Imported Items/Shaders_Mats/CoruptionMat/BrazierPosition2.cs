
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrazierPosition2 : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // sends the transform position to the Coruption shader using Global vector
        Shader.SetGlobalVector("_Brazier2Position", transform.position);
        void scaleTheRadius()
        {
            Shader.SetGlobalVector("_Braizer2Radius", new Vector4(100,0,0,0));
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Write multiple scripts for each group of waves that need to be detected when finished

public class Area2Script : AreaGate
{
   public GameObject Firearea2;
   public GameObject FireCone2;
    public GameObject Shield2;
    int StartRadius2 = 0;
   int BrazierRadius2 = 0;
    int FinalRadius2 = 0;
    // Use override keyword
    void Awake()
    {
        Instantiate(Shield2);
    }
    void Update()
    {
        if (FinalRadius2 < BrazierRadius2)
        {
            FinalRadius2++;
        }
        Shader.SetGlobalFloat("_Brazier2Radius", FinalRadius2);
    }
    public override void OpenArea()
    {
        // Stick all code
        // and methods that
        // need to be called
        // after a group of
        // waves is finished
        // in here.
        Instantiate(Firearea2);
        Instantiate(FireCone2);
        BrazierRadius2 = 30;
        Destroy(Shield2);


    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Write multiple scripts for each group of waves that need to be detected when finished

public class Area1Script : AreaGate
{
   public GameObject Firearea;
   public GameObject FireCone;
    public GameObject Shield;
    int StartRadius1 = 0;
   int BrazierRadius1 = 0;
    int FinalRadius1 = 0;
    // Use override keyword
 
    void Update()
    {
        if (FinalRadius1 < BrazierRadius1)
        {
            FinalRadius1++;
        }
        Shader.SetGlobalFloat("_Brazier1Radius", FinalRadius1);
    }
    void Awake()
    {
        Instantiate(Shield);
    }
    public override void OpenArea()
    {
        // Stick all code
        // and methods that
        // need to be called
        // after a group of
        // waves is finished
        // in here.
        Instantiate(Firearea);
        Instantiate(FireCone);
        BrazierRadius1 = 38;
        Destroy(Shield);


    }
    
}

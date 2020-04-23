using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Write multiple scripts for each group of waves that need to be detected when finished

public class ExampleAreaGateScript : AreaGate
{
   public GameObject Firearea;
   public GameObject FireCone;

    public float sizeX;
    public float sizeY;
    public float sizeZ;
    public float sizeA;
    // Use override keyword

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


      
    }

    void update()
    {
        Shader.SetGlobalVector("_Braizer1Radius", new Vector4(sizeX, sizeY, sizeZ, sizeA));
    }
}

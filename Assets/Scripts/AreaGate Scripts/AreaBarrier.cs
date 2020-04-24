using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AreaBarrier : AreaGate
{
    public NavMeshObstacle barrier;

    public override void OpenArea()
    {
        Destroy(barrier);
    }
}

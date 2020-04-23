using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AmbientSound : SoundArray
{
    public AmbientZone zone; // The AmbientZone component used to determine the volume of ambience
    public float averageSecondsPerNoise; // On average how many seconds pass between each ambient sound, set to 0 to loop
    public float randomSecondDeviation; // How random the delays can last for, set to 0 for exact timings
}

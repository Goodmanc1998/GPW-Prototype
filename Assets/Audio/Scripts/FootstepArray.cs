using System;

[System.Serializable]
public class FootstepArray
{
    public string surfaceName; // The name of the surface that will be used to play these sounds, eg. dirt, stone, wood
    public FootstepSound[] possibleSounds; // Array of possible sounds that will be played when on this surface

    public int totalWeighting { get; private set; } // Total weighting used to choose a random sound

    // Sort the array once so it dosen't need to be done every time a sound is chosen and calculate the total weighting 
    public void InitArray()
    {
        // Sort the sounds in ascending order
        Array.Sort(possibleSounds, (f1, f2) => f1.weight.CompareTo(f2.weight));

        // Sum the weights of each sound and store it
        totalWeighting = 0;
        for (int i = 0; i < possibleSounds.Length; i++)
        {
            totalWeighting += possibleSounds[i].weight;
        }
    }
}

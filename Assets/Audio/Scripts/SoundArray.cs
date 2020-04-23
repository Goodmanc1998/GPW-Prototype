using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundArray
{
    public SoundEffect[] soundArray; // Array of possible sounds that will be played when on this surface

    public int totalWeighting { get; private set; } // Total weighting used to choose a random sound

    // Sort the array once so it dosen't need to be done every time a sound is chosen and calculate the total weighting 
    public void InitArray()
    {
        // Sort the sounds in ascending order
        Array.Sort(soundArray, (f1, f2) => f1.weight.CompareTo(f2.weight));

        // Sum the weights of each sound and store it
        totalWeighting = 0;
        for (int i = 0; i < soundArray.Length; i++)
        {
            totalWeighting += soundArray[i].weight;
        }
    }

    // Chooses a weighted random sound from the surfaces possible footstep sounds
    public SoundEffect GetRandomSound()
    {
        int random = UnityEngine.Random.Range(0, totalWeighting);
        for (int i = 0, current = 0; i < soundArray.Length; i++)
        {
            current += soundArray[i].weight;
            if (random < current)
            {
                return soundArray[i];
            }
        }
        return null;
    }
}

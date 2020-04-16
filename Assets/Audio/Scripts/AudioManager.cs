using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public SoundEffect[] sounds;

    static AudioManager audioManager;

    private void Awake()
    {
        audioManager = this;
        for (int i = 0; i < sounds.Length; i++)
        {

        }
    }
}

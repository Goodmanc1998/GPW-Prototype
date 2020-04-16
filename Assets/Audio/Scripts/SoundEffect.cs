using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class SoundEffect
{
    public string name;
    public AudioClip sound;

    [Range(0f, 1f)]
    public float volume = 1;
}

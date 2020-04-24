using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Music
{
    public string name;
    public AudioClip clip;
    [Range(0, 1)]
    public float volume;

    public AudioSource source { private get; set; }

    public void InitMusic(GameObject audioManager)
    {
        source = audioManager.AddComponent<AudioSource>();

        source.clip = clip;
        source.loop = true;
        source.volume = 0f;
        source.spatialBlend = 0f;
    }

    // Fade the music by setting its volume over time
    public IEnumerator Fade(float startVolume, float endVolume, float time)
    {
        // If the music starts with a volume of 0, start the song from the beginning
        if (startVolume == 0)
        {
            source.Play();
        }

        // If time is 0, the music volume should be changed immediately
        if (time > 0)
        {
            // Interpolate the fading linearly 
            float currentTime = 0f;
            while (currentTime < time)
            {
                source.volume = Mathf.Lerp(startVolume * volume, endVolume * volume, currentTime / time);
                yield return new WaitForEndOfFrame();
                currentTime += Time.deltaTime;
            }
        }
        source.volume = endVolume * volume; // Set the volume to its final amount

        // If the music ends with a volume of 0, it should be turned off
        if (endVolume == 0)
        {
            source.Stop();
        }
    }
}

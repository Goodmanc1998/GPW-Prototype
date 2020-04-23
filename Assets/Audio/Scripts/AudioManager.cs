using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Music[] music; // Array of assigned music
    public string openingMusic; // Play music as soon as the game launches

    public AmbientSound[] ambientSounds; // Array of ambient zones used to play random ambient sounds as the player is inside them

    Music currentMusic; // The music currently playing

    private void Awake()
    {
        // Initialise the games music
        foreach (Music m in music)
        {
            m.InitMusic(gameObject);
        }

        // Initialise the games ambient sounds
        foreach (AmbientSound ambientArea in ambientSounds)
        {
            ambientArea.InitArray();
            StartCoroutine(PlayAmbience(ambientArea));
        }
    }

    private void Start()
    {
        // Play opening music if there is any
        if (openingMusic != "")
        {
            PlayMusic(openingMusic);
        }
    }

    // Play an assigned song, by fading it in and fading out the current music.
    // Set crossFade to 0 to immediately start/change the music.
    // Use forceRestartMusic to make the music restart if it is already playing, otherwise return.
    public void PlayMusic(string musicName, float crossfadeTime, bool forceRestartMusic)
    {
        Music song = Array.Find(music, m => m.name == musicName); // Find the music within the assigned musics array

        // If the song does not exist within the musics array, print an error message and return
        if (song == null)
        {
            Debug.LogError(musicName + " is not assigned to this AudioManager.");
            return;
        }

        // Handle what happens if this music is already playing
        bool restart = false;
        if (currentMusic == song)
        {
            if (forceRestartMusic)
            {
                Debug.Log(musicName + " has restarted.");
                restart = true;
            }
            else
            {
                Debug.LogWarning(musicName + " is already playing.");
                return;
            }
        }

        // If there is no current music and the new music is not to be restarted, there should be no fade out
        if (currentMusic != null && !restart)
        {
            FadeOut(crossfadeTime); // Fade out the current music
        }

        // Set the current music to the new song and fade it in
        currentMusic = song;
        FadeIn(song, crossfadeTime);
    }

    // Play an assigned song by fading it in over time
    public void PlayMusic(string musicName, float crossFadeTime)
    {
        PlayMusic(musicName, crossFadeTime, false);
    }

    // Play an assigned song
    public void PlayMusic(string musicName)
    {
        PlayMusic(musicName, 0f, false);
    }

    // Stop the current song, by fading it out over time
    public void StopMusic(float fadeTime)
    {
        FadeOut(fadeTime);
    }

    // Stop the current song
    public void StopMusic()
    {
        FadeOut(0f);
    }

    // Fade a song out over time, set to 0 to instantly stop it
    void FadeOut(float fadeTime)
    {
        StartCoroutine(currentMusic.Fade(1f, 0f, fadeTime));
    }

    // Fade a song in over time, set to 0 to instantly play it
    void FadeIn(Music music, float fadeTime)
    {
        StartCoroutine(music.Fade(0f, 1f, fadeTime));
    }

    // Play a random ambient sound every so many seconds
    IEnumerator PlayAmbience(AmbientSound sounds)
    {
        // Keep playing while the audio manager exists
        while (true && sounds.soundArray.Length > 0 && sounds.zone != null)
        {
            // If the sound is able to be heard
            if (sounds.zone.volumeRange > 0)
            {
                // Play a random sound around the listener
                SoundEffect randomSound = sounds.GetRandomSound();
                AudioSource.PlayClipAtPoint(randomSound.sound, sounds.zone.listener.gameObject.transform.position + UnityEngine.Random.onUnitSphere, randomSound.volume * sounds.zone.volumeRange);
            }
            // Wait between 50% and 150% of the average time before playing the next sound
            // Would be better to use a random distribution function instead
            yield return new WaitForSeconds(sounds.averageSecondsPerNoise * UnityEngine.Random.Range(0.5f, 1.5f));
        }
    }
}

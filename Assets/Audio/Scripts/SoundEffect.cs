using UnityEngine;

[System.Serializable]
public class SoundEffect
{
    public string name; // Name of the clip, not required and only used to differentiate sound clips
    public AudioClip sound; // The sound clip, if the the clip is for a footstep it must contain the footstep early on as often it will be cut
    public int weight; // The weight used to determine how likely this sound is to be played, the higher the weight the more likely

    [Range(0f, 1f)]
    public float volume = 1; // The volume of this individual clip
}

using UnityEngine;

[System.Serializable]
public class FootstepSound
{
    public string name; // Name of the clip, not required and only used to differentiate sound clips
    public AudioClip sound; // The sound clip for the footstep, the clip must contain the footstep early on as often it will be cut
    public int weight; // The weight used to determine how likely this sound is to be played, the higher the weight the more likely

    [Range(0f, 1f)]
    public float volume = 1; // The volume of this individual clip
}

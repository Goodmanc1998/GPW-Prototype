using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AmbientZone : MonoBehaviour
{
    public float farRange; // If the listener is beyond this distance, the sounds will not be played
    public float nearRange; // If the listener is beyond this distance, the sounds will be played at a lower volume, if the player is closer the sounds will be played at full volume
    
    [Range(0f, 1f)]
    public float volume = 1; // The max volume of the zone

    public AudioListener listener; // Audio listener, should be set as the player

    [HideInInspector]
    public AudioSource source;

    private void Awake()
    {
        source = gameObject.AddComponent<AudioSource>();
    }

    // Check if the target is in range every frame and update the volume of the ambience accordingly
    private void Update()
    {
        // If there is no listener, return
        if (listener == null)
        {
            Debug.LogWarning("AmbientZone must have an audio listener.");
            return;
        }

        // Calculate how much the volume should be set to
        float distance = Vector3.Distance(gameObject.transform.position, listener.gameObject.transform.position);
        source.volume = volume * Mathf.Clamp(1 - (distance - nearRange) / (farRange - nearRange), 0f, 1f);         
    }

    // Draw wire spheres to show the near and far ranges
    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (UnityEditor.Selection.activeObject != transform.gameObject) return;
        Gizmos.color = new Color(0.54f, 0.69f, 0.81f, 0.6f);
        Gizmos.DrawWireSphere(gameObject.transform.position, nearRange);
        Gizmos.color = new Color(0.27f, 0.53f, 0.74f, 0.8f);
        Gizmos.DrawWireSphere(gameObject.transform.position, farRange);
#endif
    }

    // Ensures both ranges are at least 0 and that the far range is always at least as large as the near range
    private void OnValidate()
    {
        farRange = Mathf.Max(farRange, nearRange);
        nearRange = Mathf.Max(nearRange, 0);
    }
}

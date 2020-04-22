using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iceBarrier : Entities
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Enemy>())
        {
            other.GetComponent<Enemy>().SB("freeze");

        }

    }
    // allows the designer to access the time that the ice barrier is on screen.
    [Header("Ice Barrier LifeTime")]
    [Tooltip("Health value of ice barrier determines how long stays on screen higher value less screen time it will have!")]
    public float depreciationRate = 12;
    [Tooltip("Freeze duration in seconds.")]
    public float freeze_Duration = 2;

    // once the enemies have been finished should put freeze duration here.
    //if collision on enemy 
    //then freezeTime = 60 * freeze_duration;
    //tell enemy how long freezeTime is.
    //inside the enemy class we will be able to handle the freezing!
    private void Update()
    {

        //using the health built into entities we repurposed it here so we could control how long the ice barrier will be on screen almost like a timer!
        health -= depreciationRate * Time.deltaTime;
        if(health < 0)
        {
            Destroy(gameObject);
        }
    }
}

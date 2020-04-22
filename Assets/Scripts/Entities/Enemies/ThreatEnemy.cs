using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ThreatEnemy : Enemy
{
    bool beginExploding;

    public float distanceBeforeExploding;

    float timeOfExplosion;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }


    // Update is called once per frame
    void Update()
    {
        if(!froze)
        {
            if (Vector3.Distance(player.position, transform.position) <= distanceBeforeExploding && beginExploding == false)
            {
                timeOfExplosion = Time.time + timeUntilExplosion;
                beginExploding = true;
                //Debug.Log("Starting Count Down");
            }
            else
            {
                SB("pursue");
            }
        }
        

        if(Time.time >= timeOfExplosion)
        {
            StartCoroutine(Explosion());
        }

        if (dead)
        {
            Destroy(this.gameObject);
        }


    }



}

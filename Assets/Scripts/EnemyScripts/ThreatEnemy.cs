using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ThreatEnemy : Entities
{
    bool beginExploding;

    public float distanceBeforeExploding;

    public float timeUntilExplosion;
    float timeOfExplosion;

    public float explosionRanage;
    public float explosionDamage;

    Material material;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        material = GetComponent<Renderer>().material;
    }


    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(player.position, transform.position) <= distanceBeforeExploding && beginExploding == false)
        {
            timeOfExplosion = Time.time + timeUntilExplosion;
            beginExploding = true;
            //Debug.Log("Starting Count Down");
            
        }
        else
        {
            agent.SetDestination(player.position);
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


    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(timeUntilExplosion);

        if (Vector3.Distance(player.position, transform.position) <= explosionRanage)
        {
            player.GetComponent<Entities>().TakeDamage(explosionDamage, "Explosion");
            Destroy(gameObject);
        }
    }
}

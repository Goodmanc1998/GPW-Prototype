using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : Entities
{
    public Transform target;
    int health;
    public int startingHealth;



    public GameObject spell;
    public Transform spellSpawnAttack;

    public bool toggleSpellAttack;
    public int attackRadius;
    public float timeBetweenAttacks;
    float timer;

    public bool toggleSlowAttack;
    public int slowDownRange;
    public float slowDownAmount;
    float startingPlayerSpeed;

    NavMeshAgent agent;

    private void Start()
    {
        health = startingHealth;
        timer = timeBetweenAttacks;

        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            startingPlayerSpeed = target.GetComponent<NavMeshAgent>().speed;
        }
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
        }

        
        if(toggleSpellAttack)
        {
            if (Vector3.Distance(transform.position, target.position) < attackRadius)
            {
                if (timer > timeBetweenAttacks)
                {
                    Destroy(Instantiate(spell, spellSpawnAttack.position, transform.rotation), 3);
                    timer = 0;
                }
                else
                {
                    timer += Time.deltaTime;
                }
            }
        }

        if(toggleSlowAttack)
        {
            if (Vector3.Distance(transform.position, target.position) < slowDownRange)
            {
                if (target.GetComponent<NavMeshAgent>() != null)
                {
                    target.GetComponent<NavMeshAgent>().speed = slowDownAmount;
                }
            }
            else
            {
                if (target.GetComponent<NavMeshAgent>() != null)
                {
                    target.GetComponent<NavMeshAgent>().speed = startingPlayerSpeed;
                }
            }
        }
    }

    public override void applyDamage(int dmgToDo)
    {
        //applies a damage to current health and returns the value;

        if (health <= dmgToDo)
        {
            Destroy(gameObject);
        }
        else
        {
            health -= dmgToDo;
        }

    }
    
    public void WaterTrap(float waitTime)
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        rb.velocity = Vector3.zero;

        bool finished = false;

        agent.enabled = false;

        float t = 0;

        while (finished != true)
        {
            if (t < waitTime)
            {
                t += Time.deltaTime;
            }
            else
            {
                agent.enabled = true;
                finished = true;

            }
        }



    }

    /*
public IEnumerator WaterTrap(float waitTime)
{
    Rigidbody rb = GetComponent<Rigidbody>();

    agent.enabled = false;
    float timer = 0;

    while(timer < waitTime)
    {
        if (timer < waitTime)
        {
            timer += Time.deltaTime;
            rb.velocity = Vector3.zero;
        }
        else
        {
            yield return null;

        }
    }
    //agent.enabled = true;

}*/

    public void Remove()
    {
        Destroy(gameObject);
    }
}

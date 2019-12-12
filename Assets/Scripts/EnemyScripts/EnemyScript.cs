using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
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
    float startingSpeed;
    float startingAcc;
    float startingAngSpeed;

    public NavMeshAgent agent;

    bool pushBack;

    Vector3 pushDirection;
    int pushAmount;

    bool pullIn;
    Vector3 pullDirection;
    int pullAmount;

    private void Start()
    {
        health = startingHealth;
        timer = timeBetweenAttacks;

        startingSpeed = agent.speed;
        startingAcc = agent.acceleration;
        startingAngSpeed = agent.angularSpeed;

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

        if(pushBack)
        {
            agent.velocity = pushDirection * pushAmount;
        }

        if(pullIn)
        {
            agent.velocity = pullDirection * pullAmount;
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

    public void applyDamage(int dmgToDo)
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

    public void GetTarget()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void PushBack(float pushTime, int pushAmt, Vector3 pushDir)
    {
        StartCoroutine(PushBackIE(pushTime, pushAmt, pushDir));

        Debug.Log("Push F Called");
    }

    IEnumerator PushBackIE(float pushTime,int pushAmt, Vector3 pushDir)
    {
        pushDirection = pushDir;
        pushAmount = pushAmt;

        Debug.Log("Push ");

        pushBack = true;

        agent.speed = 10;
        agent.angularSpeed = 0f;
        agent.acceleration = 20;

        yield return new WaitForSeconds(pushTime);

        pushBack = false;
        agent.speed = startingSpeed;
        agent.angularSpeed = startingAngSpeed;
        agent.acceleration = startingAcc;

    }

    public void PullIn(float pullTime,int pullAmt, Vector3 pullDir)
    {
        StartCoroutine(PullInIE(pullTime, pullAmt, pullDir));
    }

    IEnumerator PullInIE(float pullTime, int pullAmt, Vector3 pullDir)
    {
        pullDirection = pullDir;
        pullAmount = pullAmt;

        Debug.Log("Pull ");

        pullIn = true;

        agent.speed = 10;
        agent.angularSpeed = 0f;
        agent.acceleration = 20;

        yield return new WaitForSeconds(pullTime);

        pullIn = false;
        agent.speed = startingSpeed;
        agent.angularSpeed = startingAngSpeed;
        agent.acceleration = startingAcc;

    }


    public void Remove()
    {
        Destroy(gameObject);
    }
}

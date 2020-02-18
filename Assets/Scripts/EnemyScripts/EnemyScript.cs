using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : Entities
{
    //Target transform and NavMesh Agent
    Transform target;
    NavMeshAgent agent;

    enum EnemyState
    {
        Idle,
        Moving,
        MeleeAttack,
        Dead
    };

    EnemyState currentState;

    bool hasTarget;

    public float meleeAttackRange;
    public float meleeDamage;
    public float timeBetweenAttacks;
    float nextAttackTime;





    private float startingPlayerSpeed;
    private float startingSpeed;
    private float startingAcc;
    private float startingAngSpeed;

    

    bool pushBack;

    Vector3 pushDirection;
    int pushAmount;

    bool pullIn;
    Vector3 pullDirection;
    int pullAmount;

    private void Awake()
    {
        if (target == null || agent == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            agent = gameObject.GetComponent<NavMeshAgent>();

            hasTarget = true;

        }
    }

    protected override void Start()
    {
        base.Start();

        startingSpeed = agent.speed;
        startingAcc = agent.acceleration;
        startingAngSpeed = agent.angularSpeed;

        startingPlayerSpeed = target.GetComponent<NavMeshAgent>().speed;

        if (hasTarget == true)
        {
            currentState = EnemyState.Moving;
        }

    }

    private void Update()
    {

        if(pushBack)
        {
            agent.velocity = pushDirection * pushAmount;
        }

        if(pullIn)
        {
            agent.velocity = pullDirection * pullAmount;
        }

        if(dead == true)
        {
            currentState = EnemyState.Dead;
            
            Remove();
        }


        if(currentState == EnemyState.Moving)
        {
            agent.SetDestination(target.position);
        }

        if(Vector3.Distance(transform.position, target.position) < meleeAttackRange && currentState == EnemyState.Moving)
        {
            if(Time.time > nextAttackTime)
            {
                //currentState = enemyState.MeleeAttack;
                StartCoroutine(Attack());
                nextAttackTime += Time.time + timeBetweenAttacks;

            }
        }
        
    }

    public float attackSpeed;


    IEnumerator Attack()
    {

        currentState = EnemyState.MeleeAttack;
        agent.enabled = false;

        Vector3 startingAttackPosition = transform.position;
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        Vector3 attackPosition = target.position - dirToTarget;

        float percent = 0;

        bool hasAppliedDamage = false;

        while (percent <= 1)
        {

            if (percent >= .5f && !hasAppliedDamage)
            {
                hasAppliedDamage = true;
                target.gameObject.GetComponent<PlayerMovement>().TakeDamage(meleeDamage, "Melee");
            }

            percent += Time.deltaTime * attackSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector3.Lerp(startingAttackPosition, attackPosition, interpolation);

            yield return null;
        }

        currentState = EnemyState.Moving;
        agent.enabled = true;
    }




    public void PushBack(float pushTime, int pushAmt, Vector3 pushDir)
    {
        StartCoroutine(PushBackIE(pushTime, pushAmt, pushDir));

    }

    IEnumerator PushBackIE(float pushTime,int pushAmt, Vector3 pushDir)
    {
        pushDirection = pushDir;
        pushAmount = pushAmt;

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

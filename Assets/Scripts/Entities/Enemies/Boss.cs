using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;
using UnityEngine.SceneManagement;

public class Boss : Enemy
{
    //Floats used to store the max and min shooting range
    public float minShootRange;
    public float maxShootRange;
    bool ranagedAttack;

    float distToPlayer;

    bool bossAttack;
    bool firstFlee;
    bool secondFlee;

    bool takenHitAttack;
    public int amountofHits;
    int currentHits;

    public Transform attackPoint;
    public Transform returnPoint;
    public GameObject block;

    public Transform startSpawn;
    public Transform finishSpawn;

    public GameObject bossSpell;


    bool spawnAnimation;

    bool LookPlayer;

    public GameObject boss;
    public float offset;


    private void Awake()
    {
        entitiesAnimator = gameObject.GetComponent<Animator>();



    }

    protected override void Start()
    {
        base.Start();

        attackPoint = GameObject.FindGameObjectWithTag("attackPoint").transform;
        returnPoint = GameObject.FindGameObjectWithTag("returnPoint").transform;
        block = GameObject.FindGameObjectWithTag("block");

        startSpawn = GameObject.FindGameObjectWithTag("SpawnS").transform;
        finishSpawn = GameObject.FindGameObjectWithTag("SpawnF").transform;

        GameObject.FindGameObjectWithTag("UI").GetComponent<UIScript>().boss = this;

        LookPlayer = true;


        spawnAnimation = true;
        StartCoroutine(BossSpawn());

        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        agent.enabled = false;


    }
    private void Update()
    {
        //fight starts
        distToPlayer = Vector3.Distance(player.position, transform.position); // Calculates the distance between the player and enemy

        LookAtPlayer();

        if(Vector3.Distance(transform.position, agent.destination) < 0.5)
        {
            entitiesAnimator.SetBool("bossWalk", false);
        }


        if (!dead && !bossAttack && !spawnAnimation)
        {
            if (health < startingHealth / 100 * 66 && !firstFlee)
            {
                StartCoroutine(FleeShootAttack());   
            }

            if (health < startingHealth / 100 * 33 && !secondFlee)
            {
                StartCoroutine(FleeShootAttack());
            }

            if(takenHitAttack && !bossAttack)
            {
                StartCoroutine(DashAttack());

                takenHitAttack = false;
            }

            if(distToPlayer >= maxShootRange && !bossAttack)
            {
                SB("seek");
                entitiesAnimator.SetBool("bossWalk", true);
            }

            if(distToPlayer <= maxShootRange && distToPlayer >= minShootRange && !bossAttack)
            {
                StartCoroutine(RangedAttack());
            }

            if(distToPlayer <= meleeAttackRange && !bossAttack)
            {
                if(Time.time > timeTillNextAttack && !attacking)
                {
                    StartCoroutine(BossMelee());

                }


            }


            
           
        }
        if(dead)
        {

            StartCoroutine(LoadNewScene());
        }
    }

    IEnumerator LoadNewScene()
    {

        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene("OutroScene");


    }

    void LookAtPlayer()
    {

        Vector3 lookDir = transform.position - player.position;

        Quaternion lookRotation = Quaternion.LookRotation(-lookDir);

        Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 12).eulerAngles;

        transform.rotation = Quaternion.Euler(0.0f, rotation.y, 0.0f);

    }

    IEnumerator BossMelee()
    {

        bossAttack = true;

        entitiesAnimator.SetTrigger("bossMelee");

        yield return new WaitForSeconds(1.5f);

        StartCoroutine(MeleeAttack());



        bossAttack = false;

        yield return null;


    }

    IEnumerator DashAttack()
    {

        Debug.Log("Dash Attack");

        //Setting Dash Attack to true
        bossAttack = true;

        //If the player is still outside the melee range see
        if(distToPlayer > meleeAttackRange)
        {
            SB("pursue");

            //Updating the movement starts for the Boss to chase player
            UpdateMovementStats(movementSpeed + 5, 0, acceleration + 5);


            entitiesAnimator.SetBool("bossWalk", true);


            //Waits till the player is within melee range
            yield return new WaitUntil(() => distToPlayer < meleeAttackRange + 2);

            //Resseting the movement stats
            ResetMovementStats();

        }

        LookPlayer = true;


        //For loop to run for as many melee attacks
        StartCoroutine(BossMelee());




    }

    IEnumerator RangedAttack()
    {

        Debug.Log("Ranged Attack");

        //Setting Boss attack true
        bossAttack = true;

        //Fleeing for X seconds
        //SB("flee");

        Debug.Log("Start boss walk");

        entitiesAnimator.SetBool("bossWalk", true);
        yield return new WaitForSeconds(1.6f);


        LookPlayer = true;

        for (int i = 0; i < 2; i++)
        {
            Debug.Log("Start boss spell");

            entitiesAnimator.SetTrigger("bossSpell");

            yield return new WaitForSeconds(1f);

            StartCoroutine(Shoot());

            yield return new WaitForSeconds(1f);


        }



        //Boss attack set to false
        bossAttack = false;
        //LookPlayer = false;

        yield return null;
    }

    IEnumerator FleeShootAttack()
    {
        Debug.Log("Flee Shoot Attack");

        //Boss attack set to True
        bossAttack = true;

        //Setting the destination to the attack position untill the boss has reached the position
        if (agent.enabled == false)
        {
            agent.enabled = true;
        }

        agent.SetDestination(attackPoint.position);

        entitiesAnimator.SetBool("bossWalk", true);

        yield return new WaitUntil(() => Vector3.Distance(transform.position, attackPoint.position) <= 3);

        Debug.Log("Reached position");

        //Making the boss inaccessable
        block.GetComponent<NavMeshObstacle>().enabled = true;

        //Boss shoots X amount of spells
        for (int i = 0; i < 8; i++)
        {

            //looking at the player
            LookPlayer = true;

            entitiesAnimator.SetTrigger("bossSpell");

            yield return new WaitForSeconds(1f);



            //Creating the bos spell
            Instantiate(bossSpell, transform.position + ((transform.forward * 2) + (transform.up * 2)), Quaternion.Euler(0.0f, transform.rotation.y + 180, 0.0f));

            yield return new WaitForSeconds(1f);

        }

        //LookPlayer = false;


        //Waiting X seconds after casting spells
        yield return new WaitForSeconds(2f);

        //Disabling the block to allow boss to return to the arena 
        block.GetComponent<NavMeshObstacle>().enabled = false;

        entitiesAnimator.SetBool("bossWalk", true);

        //Moving the boss back to the arena 
        agent.SetDestination(returnPoint.position);
        yield return new WaitUntil(() => Vector3.Distance(transform.position, attackPoint.position) <= 3);

        if(!firstFlee)
        {
            firstFlee = true;

            if (!secondFlee)
                secondFlee = true;
        }


        //Boss attack to false
        bossAttack = false;

        yield return null;

    }

    public override void TakeDamage(float damageIn, string attackType)
    {
        base.TakeDamage(damageIn, attackType);

        entitiesAnimator.SetTrigger("bossDamage");

        //Storing the amount of hits 
        currentHits++;

        //Checking if boss should do taken to many hit attack
        if(currentHits >= amountofHits)
        {
            //Setting taken hit attack to true
            takenHitAttack = true;
        }
    }

    IEnumerator BossSpawn()
    {
        entitiesAnimator.SetBool("bossSpawn", true);

        float percent = 0;
        while (percent <= 1)
        {

            //Moving the player between the starting position, and attack position then back to the starting position
            percent += Time.deltaTime * 6;
            transform.position = Vector3.Lerp(startSpawn.position, finishSpawn.position, percent);

            yield return null;
        }

        agent.enabled = true;

        gameObject.GetComponent<CapsuleCollider>().enabled = true;
        yield return new WaitForSeconds(5.8f);

        entitiesAnimator.SetBool("bossSpawn", false);

        spawnAnimation = false;

        yield return null;

    }


    //Functions used for updating the bosses movement stats and restting them
    void UpdateMovementStats(float newMovement, float newAngular, float newAcceleration)
    {
        agent.speed = newMovement;
        agent.angularSpeed = newAngular;
        agent.acceleration = newAcceleration;
    }

    void ResetMovementStats()
    {
        agent.speed = movementSpeed;
        agent.angularSpeed = angularSpeed;
        agent.acceleration = acceleration;
    }
}

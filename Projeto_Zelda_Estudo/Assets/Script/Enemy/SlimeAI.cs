using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SlimeAI : MonoBehaviour
{
    private GameManager gameManager;
   
    private Animator anim;
    public int HP;
    private bool isDead;

    public enemyState state;

    private bool isWalk;
    private bool isAlert;
    private bool isPlayerVisible;
    private bool isAttack;

    //I.A
    private NavMeshAgent agent;
    private int idWayPoint;
    private Vector3 destination;


    private void Start()
    {
        gameManager = FindObjectOfType(typeof(GameManager)) as GameManager;
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        ChangeState(state);
    }

    private void Update() 
    {
        StateManager();

        if (agent.desiredVelocity.magnitude >= 0.1f)
        {
            isWalk = true;
        }
        else 
        {
            isWalk = false;
        }
        anim.SetBool("isWalk", isWalk);
        anim.SetBool("isAlert", isAlert);
    }


    IEnumerator Died()
    {
        isDead = true;
        yield return new WaitForSeconds(2.3f);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") 
        {
            isPlayerVisible = true;

            if (state == enemyState.IDLE || state == enemyState.PATROL)
            {
                ChangeState(enemyState.ALERT);
            }
            else if (state == enemyState.FOLLOW) 
            {
                StopCoroutine("FOLLOW");
                ChangeState(enemyState.FOLLOW);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player") 
        {
            isPlayerVisible = false;
        }
    }


    #region Methods

    public void GetHit(int amount) 
    {
        if (isDead == true) { return; }

        HP -= amount;

        if (HP > 0)
        {
            ChangeState(enemyState.FURY);
            anim.SetTrigger("GetHit");
        }
        else 
        {
            anim.SetTrigger("Die");
            StartCoroutine("Died");
        }
    }


    private void StateManager() 
    {
        switch (state) 
        {
            case enemyState.ALERT:
                LookAt();
                break;

            case enemyState.FOLLOW:
                LookAt();
                destination = gameManager.player.position;
                agent.destination = destination;
                if (agent.remainingDistance <= agent.stoppingDistance) 
                {
                    Attack();
                }
                break;

            case enemyState.FURY:
                LookAt();
                destination = gameManager.player.position;
                agent.destination = destination;
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    Attack();
                }
                break;

            case enemyState.PATROL:
                break;
        }
    }

    #region State Machine
    private void ChangeState(enemyState newState) 
    {
        print(newState); //Check on console
        StopAllCoroutines();       
        isAlert = false;

        switch (newState)
        {
            case enemyState.IDLE:
                agent.stoppingDistance = gameManager.slimeStopDistance;
                destination = transform.position;
                agent.destination = destination;

                StartCoroutine("IDLE");
                break;

            case enemyState.ALERT:
                agent.stoppingDistance = gameManager.slimeStopDistance;
                destination = transform.position;
                agent.destination = destination;
                isAlert = true;
                StartCoroutine("ALERT");
                break;

            case enemyState.PATROL:
                agent.stoppingDistance = gameManager.slimeStopDistance;
                idWayPoint = Random.Range(0, gameManager.slimeWayPoints.Length);
                destination = gameManager.slimeWayPoints[idWayPoint].position;
                agent.destination = destination;  
                
                StartCoroutine("PATROL"); 
                break;

            case enemyState.FOLLOW:
                agent.stoppingDistance = gameManager.slimedistanceToAttack;
                StartCoroutine("FOLLOW");
                break;

            case enemyState.FURY:
                destination = transform.position;
                agent.stoppingDistance = gameManager.slimedistanceToAttack;
                agent.destination = destination;
                
                break;
        }
        
        state = newState;
    }

    #region IEnumarators
    IEnumerator IDLE() 
    {
        yield return new WaitForSeconds(gameManager.slimeIdleWaitTime);
        StayStill(30);
    }
    IEnumerator PATROL() 
    {
        yield return new WaitUntil( () => agent.remainingDistance <= 0 );
        StayStill(25);
    }

    IEnumerator ALERT() 
    {
        yield return new WaitForSeconds(gameManager.slimeAlertTime);
        if (isPlayerVisible == true)
        {
            ChangeState(enemyState.FOLLOW);
        }
        else 
        {
            StayStill(10);
        }
    }

    IEnumerator FOLLOW() 
    {
        yield return new WaitUntil(() => !isPlayerVisible);
        print("Not Seeing");

        yield return new WaitForSeconds(gameManager.slimeAlertTime);

        StayStill(35);
    }

    IEnumerator ATTACK() 
    {
        yield return new WaitForSeconds(gameManager.slimeAttackDelay);
        isAttack = false;
    }
    #endregion

    private void StayStill(int yes) 
    {
        if (Rand() <= yes)
        {
            ChangeState(enemyState.IDLE);
        }
        else 
        {
            ChangeState(enemyState.PATROL);
        }
    }

    private int Rand() 
    {
        int rand = Random.Range(0, 100);
        return rand;
    }

    private void Attack() 
    {
        if (!isAttack && isPlayerVisible == true) 
        {
            isAttack = true;
            anim.SetTrigger("Attack");
        }
    }

    public void AttackIsDone()
    {
        StartCoroutine("ATTACK");
    }

    private void LookAt() 
    {
        Vector3 lookDirection = (gameManager.player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, gameManager.slimeLookAtSpeed * Time.deltaTime);
    }

    #endregion

    #endregion
}

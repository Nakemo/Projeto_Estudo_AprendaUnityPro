using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Slime : MonoBehaviour
{
    private Animator anim;
    public int HP;

    public enemyState state;

    public const float idleWaitTime = 3f;
    public const float patrolWaitTime = 5f;

    private bool isDead;

    private void Start()
    {
        anim = GetComponent<Animator>();
        ChangeState(state);
    }

    private void Update() 
    {
        StateManager();
    }


    IEnumerator Died()
    {
        isDead = true;
        yield return new WaitForSeconds(2.3f);
        Destroy(this.gameObject);
    }

    #region Methods

    public void GetHit(int amount) 
    {
        if (isDead == true) { return; }

        HP -= amount;

        if (HP > 0)
        {
            anim.SetTrigger("GetHit");
        }
        else 
        {
            anim.SetTrigger("Die");
            StartCoroutine("Died");
        }
    }

    #region State Machine
    private void StateManager() 
    {
        switch (state) 
        {
            case enemyState.IDLE:
                break;

            case enemyState.ALERT:
                break;

            case enemyState.EXPLORE:
                break;

            case enemyState.FOLLOW:
                break;

            case enemyState.FURY:
                break;

            case enemyState.PATROL:
                break;
        }
    }

    private void ChangeState(enemyState newState) 
    {
        print(newState); //Check on console
        StopAllCoroutines();       
        state = newState;

        switch (state)
        {
            case enemyState.IDLE:                
                StartCoroutine("IDLE");
                break;

            case enemyState.ALERT:

                break;

            case enemyState.PATROL:
                StartCoroutine("PATROL");
                break;
        }
    }

    #region IEnumarators
    IEnumerator IDLE() 
    {
        yield return new WaitForSeconds(idleWaitTime);
        if (Rand() < 50)
        {
            ChangeState(enemyState.IDLE);
        }
        else 
        {
            ChangeState(enemyState.PATROL);
        }
    }
    IEnumerator PATROL() 
    {
        yield return new WaitForSeconds(patrolWaitTime);
        ChangeState(enemyState.IDLE);
    }
    #endregion

    private int Rand() 
    {
        int rand = Random.Range(0, 100);
        return rand;
    }
    #endregion

    #endregion
}

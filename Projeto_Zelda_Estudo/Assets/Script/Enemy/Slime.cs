using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    private Animator anim;
    public int HP;

    private bool isDead;

    private void Start()
    {
        anim = GetComponent<Animator>();
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

    #endregion
}

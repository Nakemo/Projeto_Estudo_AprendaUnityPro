using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    public ParticleSystem fxHit;
    private bool isCut;

    public void GetHit(int amount)
    {
        if (!isCut) 
        {
            isCut = true;
            transform.localScale = new Vector3(1f, 1f, 1f);
            fxHit.Emit(10);
            StartCoroutine("GrowGrass");
        }
    }
    
    IEnumerator GrowGrass()
    {
        yield return new WaitForSeconds(3f);
        transform.localScale = new Vector3(3f, 3f, 3f);
        isCut = false;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerIK : MonoBehaviour
{
    Animator animator;
    public bool ikActive = false;
    public Transform objTarget;
    public float lookWeight;
    public float desireDist;


    GameObject objPivot;

    
    void Start()
    {
        animator = GetComponent<Animator>();

        objPivot = new GameObject("LookPivot");
        objPivot.transform.parent = transform.parent;
        objPivot.transform.localPosition = new Vector3(-1.11f, 1.19f, 33f);
    }

    
    void Update()
    {
        objPivot.transform.LookAt(objTarget);
        float pivotRotY = objPivot.transform.localRotation.y;
        Debug.Log(pivotRotY);

        float dist = Vector3.Distance(objPivot.transform.position, objTarget.position);

        if (pivotRotY < 0.58f && pivotRotY > -0.45f && dist < desireDist)
        {
            lookWeight = Mathf.Lerp(lookWeight, 1, Time.deltaTime * 2.5f);
        }
        else 
        {
            lookWeight = Mathf.Lerp(lookWeight, 0, Time.deltaTime * 2.5f);
        }
    }


    private void OnAnimatorIK(int LookAt)
    {
        if (animator) 
        {
            if (ikActive)
            {
                if (objTarget != null)
                {
                    animator.SetLookAtWeight(lookWeight);
                    animator.SetLookAtPosition(objTarget.position);
                }
            }
            else 
            {
                animator.SetLookAtWeight(0);
            }
        }
    }

}

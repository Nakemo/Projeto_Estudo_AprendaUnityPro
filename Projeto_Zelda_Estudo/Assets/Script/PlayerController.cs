using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController chrController;
    private Animator anim;


    [Header("Config Player")]
    public float movementSpeed = 3f;

    private Vector3 direction;
    private bool isWalk;


    void Start()
    {
        chrController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Fire1")) 
        {
            anim.SetTrigger("Attack");
        }



        direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude > 0.1f)
        {
            //calculando o angulo em radiano e convertendo para graus.
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, targetAngle, 0);
            isWalk = true;
        }
        else 
        {
            isWalk = false;
        }

        chrController.Move(direction * movementSpeed * Time.deltaTime);

        anim.SetBool("isWalk", isWalk);

    }
}

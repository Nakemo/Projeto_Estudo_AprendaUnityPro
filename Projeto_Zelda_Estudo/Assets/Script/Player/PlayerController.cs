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


    [Header("Player Stats")]
    public int HP = 10;

    //Inputs
    public float horizontal;
    public float vertical;

    [Header("Attack Config")]
    public ParticleSystem fxAttack;
    public Transform hitBox;
    [Range(0.2f, 1f)]
    public float hitRange = 0.5f;

    public LayerMask hitMask;

    private bool isAttack;
    [SerializeField]
    Collider[] hitInfo;
    public int amountDmg;

    [Header("Player Audio")]
    public AudioSource attackAudio;

  
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        chrController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Inputs();
        PlayerMovements();
        UpdateAnimator();
    }

    #region Methods

    public void Inputs()     
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Fire1") && !isAttack)
        {
            Attack();
            attackAudio.Play();
        }        
    }

    public void Attack() 
    {
        isAttack = true;
        anim.SetTrigger("Attack");
        fxAttack.Emit(1);

        hitInfo = Physics.OverlapSphere(hitBox.position, hitRange, hitMask);

        foreach (Collider c in hitInfo) 
        {
            c.gameObject.SendMessage("GetHit", amountDmg, SendMessageOptions.DontRequireReceiver);
        }
    }

    public void PlayerMovements() 
    {
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
    }

    public void UpdateAnimator() 
    {
        anim.SetBool("isWalk", isWalk);
    }


    public void AttackIsDone() 
    {
        isAttack = false;
    }
    #endregion


    #region Health Parameters 
    //This segment will be fixed and moved for a separated Script, in which it will be able to see the player health in a screen UI

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "TakeDamage")
        {
            GetHit(1);
        }
    }

    private void GetHit(int amount) 
    {
        HP -= amount;
        if (HP > 0)
        {
            anim.SetTrigger("Hit");
        }
        else 
        {
            anim.SetTrigger("Die");
        }
    }
    #endregion


    private void OnDrawGizmosSelected()
    {
        if(hitBox != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(hitBox.position, hitRange);
        }
    }

}

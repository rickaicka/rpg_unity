using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float TotalHealth = 100f;
    public float CurrentHealth;
    public float AttackDamage;
    public float MovementSpeed;
    public float Speed = 3f;

    public float RotSpeed = 40f;

    private float Rotation;

    public float Gravity = 120f;


    private CharacterController controller;
    private Animator animator;
    private Vector3 MoveDirection;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //MoveEnemy();
    }

    public void GetHit()
    {
        // Debug.Log("Morri");
        // Destroy(gameObject);
        
        InitiateTransitions();
        animator.SetInteger("transition", 2);
        animator.SetBool("isDead", true);
    }

    // private void MoveEnemy()
    // {
    //     if (controller.isGrounded)
    //     {
    //         if (!animator.GetBool("isAttacking"))
    //         {
    //             if (Input.GetKey(KeyCode.W))
    //             {
    //                 animator.SetBool("isWalking", true);
    //                 animator.SetInteger("transition", 3);
    //                 MoveDirection = Vector3.forward * Speed;
    //                 MoveDirection = transform.TransformDirection(MoveDirection);
    //             }
    //             if (Input.GetKey(KeyCode.S))
    //             {
    //                 animator.SetBool("isWalking", true);
    //                 animator.SetInteger("transition", 3);
    //                 MoveDirection = Vector3.back * Speed;
    //                 MoveDirection = transform.TransformDirection(MoveDirection);
    //             }
    //             if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
    //             {
    //                 InitiateTransitions();
    //             }
    //         }
    //         else
    //         {
    //             InitiateTransitions();
    //             StartCoroutine(Attack());
    //         }
    //     }
    //     Rotation += Input.GetAxis("Horizontal") * RotSpeed * Time.deltaTime;
    //     transform.eulerAngles = new Vector3(0, Rotation, 0);
    //     
    //     MoveDirection.y -= Gravity * Time.deltaTime;
    //     controller.Move(MoveDirection * Time.deltaTime);
    // }
    
    private void InitiateTransitions()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isDead", false);
        animator.SetInteger("transition", 0);
        MoveDirection = Vector3.zero;
    }
    
    IEnumerator Attack()
    {
        animator.SetBool("isAttacking", true);
        animator.SetInteger("transition", 1);
        yield return new WaitForSeconds(2f);
        
        animator.SetInteger("transition", animator.GetBool("isWalking") ? 3 : 0);
        animator.SetBool("isAttacking", false);
    }
}

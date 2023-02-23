using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 5f;

    public float RotSpeed = 40f;

    private float Rotation;

    public float Gravity = 120f;

    private Vector3 MoveDirection;

    private CharacterController controller;

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        GetMouseInput();
    }

    private void MovePlayer()
    {
        if (controller.isGrounded)
        {
            if (!animator.GetBool("isAttacking"))
            {
                if (Input.GetKey(KeyCode.W))
                {
                    animator.SetBool("isWalking", true);
                    animator.SetInteger("transition", 1);
                    MoveDirection = Vector3.forward * Speed;
                    MoveDirection = transform.TransformDirection(MoveDirection);
                }
                if (Input.GetKey(KeyCode.S))
                {
                    animator.SetBool("isWalking", true);
                    animator.SetInteger("transition", 1);
                    MoveDirection = Vector3.back * Speed;
                    MoveDirection = transform.TransformDirection(MoveDirection);
                }
                if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
                {
                    InitiateTransitions(0);
                }
            }
            else
            {
                InitiateTransitions(0);
                StartCoroutine(Attack());
            }
        }
        Rotation += Input.GetAxis("Horizontal") * RotSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, Rotation, 0);
        
        MoveDirection.y -= Gravity * Time.deltaTime;
        controller.Move(MoveDirection * Time.deltaTime);
    }

    private void GetMouseInput()
    {
        if (controller.isGrounded)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if(animator.GetBool("isWalking"))
                {
                    InitiateTransitions(1);
                }
                
                if(!animator.GetBool("isWalking"))
                {
                    StartCoroutine(Attack());
                }
            }
        }
    }

    private void InitiateTransitions(int transition)
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", false);
        animator.SetInteger("transition", transition);
        MoveDirection = Vector3.zero;
    }
    
    IEnumerator Attack()
    {
        animator.SetBool("isAttacking", true);
        animator.SetInteger("transition", 2);
        yield return new WaitForSeconds(2f);
        InitiateTransitions(animator.GetBool("isWalking") ? 1 : 0);
    }
}

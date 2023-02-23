using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 3f;

    public float RotSpeed = 40f;

    private float Rotation;

    public float Gravity = 120f;

    private Vector3 MoveDirection;

    private CharacterController controller;

    private Animator animator;

    private List<Transform> EnemiesList = new List<Transform>();

    public float ColliderRadius;

    private bool IsReady;
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
                    
                    //transform.Rotate(Vector3.up * Time.deltaTime * RotSpeed);
                    
                    
                    // Rotation += RotSpeed * Time.deltaTime;
                    // transform.rotation = Quaternion.Euler(0, Mathf.LerpAngle(transform.rotation.eulerAngles.y, RotSpeed, Rotation), 0);
                }
                if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
                {
                    InitiateTransitions();
                }
            }
            else
            {
                InitiateTransitions();
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
                    InitiateTransitions();
                }
                
                if(!animator.GetBool("isWalking"))
                {
                    StartCoroutine(Attack());
                }
            }
        }
    }

    private void InitiateTransitions()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", false);
        animator.SetInteger("transition", 0);
        MoveDirection = Vector3.zero;
    }

    private void GetEnemiesRange()
    {
        EnemiesList.Clear();
        foreach (Collider collider in Physics.OverlapSphere((transform.position + transform.forward * ColliderRadius), ColliderRadius))
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                EnemiesList.Add(collider.transform);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
       Gizmos.color = Color.red;
       Gizmos.DrawWireSphere(transform.position + transform.forward, ColliderRadius);
    }

    IEnumerator Attack()
    {
        animator.SetBool("isAttacking", true);
        animator.SetInteger("transition", 2);
        yield return new WaitForSeconds(2f);
        
        GetEnemiesRange();

        foreach (Transform enemies in EnemiesList)
        {
            //execute attack on enemy
            EnemyController enemy = enemies.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.GetHit();
            }
        }
        
        animator.SetInteger("transition", animator.GetBool("isWalking") ? 1 : 0);
        animator.SetBool("isAttacking", false);
    }
}

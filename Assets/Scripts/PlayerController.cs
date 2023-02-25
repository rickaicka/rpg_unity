using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 4f;

    public float RotSpeed = 80f;

    private float Rotation;

    public float Gravity = 120f;

    private Vector3 MoveDirection;

    private CharacterController controller;

    private Animator animator;

    private List<Transform> EnemiesList = new List<Transform>();

    public float ColliderRadius;
    public float PlayerDamage = 25f;

    private bool IsReady;
    public float TotalHealth = 100f;
    public float CurrentHealth;
    public bool PlayerIsDead;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        CurrentHealth += TotalHealth;
        PlayerIsDead = false;
        ColliderRadius = 1f;
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
                    animator.SetBool("isWalking", false);
                    animator.SetInteger("transition", 0);
                    MoveDirection = Vector3.zero;
                }
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
                animator.SetBool("isWalking", false);
                animator.SetInteger("transition", 0);
                MoveDirection = Vector3.zero;
                
                SetAttack();
            }
        }
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

    //Validate if the player is ready to attack
    IEnumerator Attack()
    {
        if(!IsReady)
        {
            IsReady = true;
            yield return new WaitForSeconds(1f);
            GetEnemiesRange();
            foreach (Transform enemies in EnemiesList)
            {
                EnemyController enemy = enemies.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    enemy.GetHit(PlayerDamage);
                }
            }
            animator.SetInteger("transition", animator.GetBool("isWalking") ? 1 : 0);
            animator.SetBool("isAttacking", false);
            IsReady = false;
        }
    }
    
    IEnumerator RecoveryFromHit() 
    {
        yield return new WaitForSeconds(1f);
        animator.SetInteger("transition", 0);
    }
    
    public void GetHit(float Damage) 
    {
        CurrentHealth -= Damage;
        DieOrHit();
        PlayerIsDead = animator.GetBool("isDead");
    }

    void DieOrHit() 
    {
        if (CurrentHealth <= 0)
        {
            animator.SetBool("isAttacking", false);
            animator.SetBool("isWalking", false);
            animator.SetBool("isDead", true);
            animator.SetInteger("transition", 4);
        }
        else
        {
            SetRecovery();
        }
    }
    
    //Function to Initiate Attack or Stop Attack if Player is Dead
    private void SetAttack()
    {
        animator.SetInteger("transition", 2);
        animator.SetBool("isAttacking", true);
        animator.SetBool("isWalking", false);
    }

    private void SetRecovery()
    {
        animator.SetInteger("transition", 3);
    }
    
    
    //Function to start Attack Animation Event
    public void AttackAnimationEvent() 
    {
        StartCoroutine(Attack());
    }
    public void RecoveryHitAnimationEvent()
    {
        StartCoroutine(RecoveryFromHit());
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float TotalHealth = 100f;
    public float CurrentHealth = 0;
    public float AttackDamage;
    public float MovementSpeed;
    public float Speed = 3f;
    public float RotSpeed = 40f;
    public float Gravity = 120f;
    public float lookRadius = 10f;
    public Transform target;

    private float Rotation;
    private CharacterController controller;
    private Animator animator;
    private Vector3 MoveDirection;
    private MeshCollider _meshCollider;
    private NavMeshAgent _navMeshAgent;
    
    
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        CurrentHealth += TotalHealth;
        _meshCollider = GetComponent<MeshCollider>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (!animator.GetBool("isDead"))
        {
            float distance = Vector3.Distance(target.position, transform.position);
            if (distance <= lookRadius)
            {
                _navMeshAgent.isStopped = false;
                if (!animator.GetBool("isAttacking"))
                {
                    _navMeshAgent.SetDestination(target.position);
                    animator.SetInteger("transition", 4);
                    animator.SetBool("isWalking", true);
                }
            
                if (distance <= _navMeshAgent.stoppingDistance)
                {
                    FaceTarget();
                    StartCoroutine(Attack());
                }
            }else{
                animator.SetInteger("transition", 0);
                animator.SetBool("isWalking", false);
                animator.SetBool("isAttacking", false);
                _navMeshAgent.isStopped = true;
            }
        }
        else
        {
            _navMeshAgent.isStopped = true;
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", false);
            animator.SetInteger("transition", 2);
            
        }
    }

    private void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    public void GetHit(float Damage)
    {
        CurrentHealth -= Damage;
        DieOrHit();
    }
    
    IEnumerator Attack()
    {
        animator.SetBool("isAttacking", true);
        animator.SetBool("isWalking", false);
        animator.SetInteger("transition", 1);
        yield return new WaitForSeconds(1f);
    }

    IEnumerator RecoveryFromHit()
    {
        yield return new WaitForSeconds(2f);
        animator.SetInteger("transition", 0);
    }

    void DieOrHit()
    {
        if (CurrentHealth <= 0)
        {
            animator.SetBool("isAttacking", false);
            animator.SetBool("isWalking", false);
            animator.SetBool("isDead", true);
            animator.SetInteger("transition", 2);
            _meshCollider.enabled = false;
            Destroy(gameObject, 4f);
        }
        else
        {
            animator.SetInteger("transition", 5);
            StartCoroutine(RecoveryFromHit());
        }
    }
}

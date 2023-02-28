using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
    public float ColliderRadius;

    private float Rotation;
    private CharacterController controller;
    private Animator animator;
    private Vector3 MoveDirection;
    private MeshCollider _meshCollider;
    private NavMeshAgent _navMeshAgent;
    private bool IsReady;
    [SerializeField] public bool PlayerIsDead;

    public Slider healthBar;
    
    [Header("Path")]
    public List<Transform> PathPoints = new List<Transform>();
    public int CurrentPoint = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        CurrentHealth += TotalHealth;
        _meshCollider = GetComponent<MeshCollider>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        PlayerIsDead = false;
        ColliderRadius = 1f;
    }

    void MoveToNextPoint()
    {
        if (PathPoints.Count > 0)
        {
            float distance = Vector3.Distance(PathPoints[CurrentPoint].position, transform.position);
            _navMeshAgent.destination = PathPoints[CurrentPoint].position;
            
            if(distance <= _navMeshAgent.stoppingDistance)
            {
                CurrentPoint = Random.Range(0, PathPoints.Count);
                CurrentPoint %= PathPoints.Count;
            }
        }
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
                    SetAttack();
                }
                else
                {
                    animator.SetBool("isAttacking", false);
                }
            }else
            {
                //StopAnimation();
                //_navMeshAgent.isStopped = true;
                
                animator.SetInteger("transition", 3);
                animator.SetBool("isWalking", true);
                MoveToNextPoint();
            }
        }
        else
        {
            animator.SetInteger("transition", 2);
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", false);
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
        healthBar.value = CurrentHealth / TotalHealth;
        DieOrHit();
    }
    
    private void StopAnimation()
    {
        animator.SetInteger("transition", 0);
        animator.SetBool("isAttacking", false);
        animator.SetBool("isWalking", false);
    }
    
    IEnumerator Attack()
    {
        if (!IsReady)
        {
            IsReady = true;
            yield return new WaitForSeconds(.9f);
            GetEnemy();
            IsReady = false;
        }
    }

    IEnumerator RecoveryFromHit()
    {
        yield return new WaitForSeconds(.5f);
        animator.SetInteger("transition", 0);
        animator.ResetTrigger("hitted");
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
            animator.SetBool("isAttacking", false);
            animator.SetBool("isWalking", false);
            animator.SetTrigger("hitted");
        }
    }

    
    //Function to Get Enemy and Deal Damage
    private void GetEnemy()
    {
        foreach (Collider collider in Physics.OverlapSphere((transform.position + transform.forward * ColliderRadius), ColliderRadius))
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                collider.gameObject.GetComponent<PlayerController>().GetHit(15f);
                PlayerIsDead = collider.gameObject.GetComponent<PlayerController>().PlayerIsDead;
            }
        }
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

    //Function to Initiate Attack or Stop Attack if Player is Dead
    private void SetAttack()
    {
        if (PlayerIsDead)
        {
            StopAnimation();
            _navMeshAgent.isStopped = true;
        }
        else
        {
            animator.SetInteger("transition", animator.GetBool("hitted") ? 5 : 1);
            animator.SetBool("isAttacking", animator.GetBool("hitted") ? false : true);
            animator.SetBool("isWalking", false);
        }
    }
}

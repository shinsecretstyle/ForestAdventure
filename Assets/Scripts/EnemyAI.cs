using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform Target;
    private NavMeshAgent navMeshAgent;

    private Animator animator;

    public float detectionRange = 30f;
    public float patrolRange = 5f;
    public float patrolWaitTime = 2f;

    private Vector3 patrolPoint;
    private bool isPatrolling;
    private float patrolTimer;

    private float attackRange = 6f;

    bool canATK;
    float atkCD;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        navMeshAgent = GetComponent<NavMeshAgent>();
        isPatrolling = false;
        patrolTimer = patrolWaitTime;

        SetRandomPatrolPoint();

        if (Target == null || navMeshAgent == null)
        {
            Debug.Log("Target or NavMeshAgent not set!");
            enabled = false;
        }

        canATK = false;
        atkCD = 1;

    }

    // Update is called once per frame
    void Update()
    {
        if (CanSeePlayer())
        {
            
            ChasePlayer();
            
        }
        else if (!isPatrolling)
        {
            
            StartPatrolling();
        }

        if (isPatrolling)
        {
            
            Patrol();
        }
        
    }

    
    bool CanSeePlayer()
    {
        if(Vector3.Distance(transform.position, Target.position) < detectionRange)
        {

            Vector3 directionToPlayer = (Target.position - transform.position).normalized;
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            if (angleToPlayer <= 90f)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, (Target.position - transform.position).normalized, out hit, detectionRange))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        
                        return true;

                    }
                }
            }
        }
        transform.localScale = new Vector3(1f, 1f, 1f);
        return false;
    }

    void ChasePlayer()
    {
        if (Vector3.Distance(transform.position, Target.position) > attackRange)
        {
            
            navMeshAgent.SetDestination(Target.position);
            canATK = false;
            animator.SetBool("Walk", true);
        }
        else
        {
            navMeshAgent.SetDestination(transform.position);
            animator.SetBool("Walk", false);
            canATK = true;
            
            //Attack();
        }

        isPatrolling = false;
    }

    void StartPatrolling()
    {
        isPatrolling = true;
        SetRandomPatrolPoint();
    }

    void Patrol()
    {
        
        navMeshAgent.SetDestination(patrolPoint);
        animator.SetBool("Walk", true);

        if (navMeshAgent.remainingDistance < 0.5f)
        {
            animator.SetBool("Walk", false);
            patrolTimer -= Time.deltaTime;
            if (patrolTimer <= 0f)
            {
                SetRandomPatrolPoint();
                patrolTimer = patrolWaitTime;
                
            }
        }
    }

    void SetRandomPatrolPoint()
    {
       
        patrolPoint = transform.position + Random.insideUnitSphere * patrolRange;
        patrolPoint.y = transform.position.y;
    }

    void Attack()
    {
        if (atkCD <= 1)
        {
            animator.SetTrigger("Attack");

            atkCD += 1;
        }
        if (atkCD > 0) 
        {
            atkCD -= Time.deltaTime;
            if (atkCD < 1.3)
            {

            }
        }
    }
    
    
}

using System.Collections;
using System.Collections.Generic;
//using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAiLite : MonoBehaviour
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
    public bool canLimit = true;
    private float attackRange = 5f;

    bool canATK;
    float atkCD;
    bool isAttacking;
    float AttackTimer;

    public int Hp;
    int MaxHp;

    public Slider HpSlider;


    bool isDie;
    float dieTime = 1.2f;

    public GameObject FallenItem;
    public GameObject slashObj;
    public GameObject limitObj;
    //private EnemyWeapon weapon;
    // Start is called before the first frame update
    void Start()
    {
        Hp = 300;
        MaxHp = Hp;
        HpSlider.maxValue = MaxHp;
        HpSlider.value = Hp;
        animator = GetComponent<Animator>();

        navMeshAgent = GetComponent<NavMeshAgent>();
        isPatrolling = false;
        patrolTimer = patrolWaitTime;

        //SetRandomPatrolPoint();

        if (Target == null || navMeshAgent == null)
        {
            Debug.Log("Target or NavMeshAgent not set!");
            enabled = false;
        }

        canATK = false;
        atkCD = 1;


        isDie = false;
    }

    // Update is called once per frame
    void Update()
    {
        HpSlider.value = Hp;


        ChasePlayer();

        if (Hp <= 0)
        {

            animator.SetBool("Die", true);
            dieTime -= Time.deltaTime;
            if (dieTime < 0)
            {
                Instantiate(FallenItem);
                Destroy(gameObject);
            }

        }

        if(isAttacking)
        {
            AttackTimer-= Time.deltaTime;
            if (AttackTimer < 0)
            {
                isAttacking = false;
            }
        }


    }


    bool CanSeePlayer()
    {
        if (Vector3.Distance(transform.position, Target.position) < detectionRange)
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

        float distanceToPlayer = Vector3.Distance(transform.position, Target.position);

        //Debug.Log("Distance to player: " + Vector3.Distance(transform.position, Target.position));
        if(distanceToPlayer > 13f && canLimit)
        {
            navMeshAgent.ResetPath();
            canLimit = false;
            animator.SetTrigger("Limit");
            StartCoroutine(resetCD(3f));

        }
        else if (distanceToPlayer > attackRange)
        {

            navMeshAgent.SetDestination(Target.position);
            canATK = false;
            animator.SetBool("Walk", true);
        }
        else if (distanceToPlayer < attackRange * 0.6f)
        {
            Vector3 moveDirection = (transform.position - Target.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, transform.position + moveDirection, Time.deltaTime * navMeshAgent.speed);
            canATK = false;
            animator.SetBool("Walk", false);
            Attack();
        }
        else
        {
            navMeshAgent.ResetPath();
            transform.LookAt(Target.position);
            animator.SetBool("Walk", false);
            canATK = true;

            Attack();
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
            isAttacking = true;
            AttackTimer = 0.8f;
            atkCD += 1.6f;
        }
        if (atkCD > 0)
        {
            atkCD -= Time.deltaTime;
            if (atkCD < 1.3)
            {
            }
        }
    }

    void ApplyDamage(int atk)
    {
        Hp -= atk;
        Debug.Log("got " + atk + " damage");
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        
    }

    //public void Slash()
    //{
    //    Instantiate(slashObj, slashPoz.position, Quaternion.identity);
    //}

    IEnumerator Slash()
    {
        slashObj.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        slashObj.SetActive(false);
    }

    public void Limit()
    {
        Instantiate(limitObj,Target.transform.position, Quaternion.identity);
    }

    IEnumerator resetCD(float time)
    {
        yield return new WaitForSeconds(time);
        canLimit = true;
    }
}

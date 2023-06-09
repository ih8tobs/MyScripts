using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UIElements;


public class AiWeapons : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public Transform Lookat;
    public LayerMask whatIsGround, whatIsPlayer;
    public float health;
    private Animator hmAnimator;
    public int damage = 1;
    public GameObject Throw;
    public GameObject Ragdoll;
    private bool isDestroyed = false;

    

    [Header("Patrolling")]
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    [Header("Attacking")]
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;
    [SerializeField] public float BulletSpeed = 10f;
    [SerializeField] public float BulletUpForce = 2f;

    public AudioSource audioSource;


    [SerializeField] private AudioClip[] clips;
    private int clipIndex;
    public AudioSource audioSource3;

    [SerializeField] private AudioClip[] clips1;
    private int clipIndex1;
    public AudioSource audioSource2;
    private bool audioplaying = false;

    [SerializeField] private AudioClip[] clips2;
    private int clipIndex2;
    public AudioSource audioSource4;
    

    [Header("States")]
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public float startTimer = 2f;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    public NavMeshAgent myNavMeshAgent;

    private void Start()
    {
        myNavMeshAgent = GetComponent<NavMeshAgent>();
        hmAnimator = GetComponent<Animator>();

    }
    private void Update()
    {
        //check sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();

        if (playerInSightRange && !audioplaying)
        {
            clipIndex1 = Random.Range(0, clips1.Length - 1);
            audioSource2.clip = clips1[clipIndex1];
            audioSource2.Play();
            audioplaying = true;


        }
    }

    private void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();
        hmAnimator.SetBool("SRunning", true);
        hmAnimator.SetBool("Idle", false);
        
        audioplaying = false;

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
            hmAnimator.SetBool("SRunning", true);
            hmAnimator.SetBool("Idle", false);
            
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //if walkpoint is reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
            hmAnimator.SetBool("SRunning", false);
            hmAnimator.SetBool("Idle", true);
            
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;

        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        hmAnimator.SetBool("SRunning", true);
        hmAnimator.SetBool("Idle", false);
        

    }

    private void AttackPlayer()
    {
        //stop enemy movement
        agent.SetDestination(transform.position);
        hmAnimator.SetBool("SRunning", false);
        hmAnimator.SetBool("Idle", false);
        hmAnimator.SetBool("Throwing", true);


        transform.LookAt(new Vector3(Lookat.position.x, transform.position.y, Lookat.position.z));

        Invoke(nameof(StartAttack), startTimer);

    }

    private void StartAttack()
    {
        if (!alreadyAttacked)
        {
            Rigidbody rb = Instantiate(projectile, Throw.transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * BulletSpeed, ForceMode.Impulse);
            rb.AddForce(transform.up * BulletUpForce, ForceMode.Impulse);
            clipIndex2 = Random.Range(0, clips2.Length);
            audioSource4.clip = clips2[clipIndex2];
            audioSource4.Play();

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }


    private void ResetAttack()
    {
        alreadyAttacked = false;
        hmAnimator.SetBool("Throwing", false);
    }

    public void TakeDamage()
    {
        health -= damage;
        clipIndex = Random.Range(0, clips.Length - 1);
        audioSource3.clip = clips[clipIndex];
        audioSource3.Play();

        if (health <= 0)
        {
            Invoke(nameof(DestroyEnemy), .5f);
        }
    }

    private void DestroyEnemy()
    {

        if (!isDestroyed)
        {
            isDestroyed = true;
            Debug.Log("FUCK");
            Destroy(gameObject);
            Instantiate(Ragdoll, transform.position, transform.rotation);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isDestroyed)
        {
            health -= damage;
            clipIndex = Random.Range(0, clips.Length - 1);
            audioSource3.clip = clips[clipIndex];
            audioSource3.Play();

            if (health <= 0)
            {
                Invoke(nameof(DestroyEnemy), .5f);
            }
        }

    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

}

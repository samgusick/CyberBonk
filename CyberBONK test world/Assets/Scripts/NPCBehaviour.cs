using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class NPCBehaviour : MonoBehaviour
{
    public static bool attacking;
    public bool isAlive;
    bool hasRoute;
    public NavMeshAgent agent;

    public Transform playerReference;
    public LayerMask environment;
    public FirstPersonAIO FirstPersonAIO;
    public float inBoundsDistance = 150f;
    public float destinationRadius = 100f;
    public float playerApproachDistance = 10f;


    public Animator animator;

    public float RerouteAtThisDistance = 5f;
    float health;
    float startingHealth = 100;

    IEnumerator waitCoroutine;
    SkinnedMeshRenderer skinnedMesh;
    Collider[] colliders;
    Rigidbody[] rigidbodies;

    void die()
    {

        if (agent.isActiveAndEnabled)
        {
            attacking = true;
            agent.isStopped = true;
            agent.enabled = false;
            animator.enabled = false;
        }


        isAlive = false;
        foreach (var item in rigidbodies)
        {
            item.isKinematic = false;
        }
    }
    public void takeDamage(float damageTaken)
    {
        health -= damageTaken;
        health = Mathf.Clamp(health, 0, startingHealth);

        if (health == 0 && isAlive)
        {
            die();
        }

    }
    private void Awake()
    {
        hasRoute = false;
        isAlive = true;
        health = startingHealth;

        playerReference = playerMovement.player;
        FirstPersonAIO = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonAIO>();
        agent = GetComponent<NavMeshAgent>();
        colliders = GetComponentsInChildren<Collider>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        skinnedMesh = GetComponentInChildren<SkinnedMeshRenderer>();
        newRoute();

        foreach (var item in rigidbodies)
        {
            item.isKinematic = true;
        }

    }

    void hideNPC()
    {
        skinnedMesh.enabled = false;
        foreach (var item in colliders)
        {
            item.enabled = false;
        }
        agent.isStopped = true;
    }

    void unhideNPC()
    {
        skinnedMesh.enabled = true;
        foreach (var item in colliders)
        {
            item.enabled = true;
        }
        agent.isStopped = false;
    }

    IEnumerator waitThanNewRoute()
    {
        yield return new WaitForSeconds(15);
        newRoute();
        yield break;
    }
    public void Update()
    {



        if (isAlive && agent.isOnNavMesh && !attacking)
        {
            animator.SetFloat("moveSpeed", agent.velocity.magnitude);
            animator.SetBool("isAttacking", false);
            if (agent.velocity.magnitude > 1f)
            {
                animator.SetBool("isMoving", true);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }

            if (agent.remainingDistance < RerouteAtThisDistance && waitCoroutine == null)
            {
                hasRoute = false;
                waitCoroutine = waitThanNewRoute();
                StartCoroutine(waitCoroutine);
            }

            if (Vector3.Distance(transform.position, FirstPersonAIO.player.position) > inBoundsDistance)
            {
                hideNPC();
            }
            else
            {
                unhideNPC();
            }
        }

        else if (isAlive && agent.isOnNavMesh && attacking)
        {
            if (waitCoroutine != null)
            {
                StopCoroutine(waitCoroutine);
            }
            animator.SetFloat("moveSpeed", agent.velocity.magnitude);
            animator.SetBool("isAttacking", true);
        
            if (agent.velocity.magnitude > 1f)
            {
                animator.SetBool("isMoving", true);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }

            if (Vector3.Distance(transform.position, FirstPersonAIO.player.position) > inBoundsDistance)
            {
                hideNPC();
            }
            else
            {
                unhideNPC();
            }
            attack_getToPlayer();
        }


    }

    public void attack_getToPlayer()
    {
        transform.LookAt(FirstPersonAIO.player.position);
        if (Vector3.Distance(transform.position, FirstPersonAIO.player.position) > playerApproachDistance && agent.destination != FirstPersonAIO.player.position)
        {
            agent.SetDestination(FirstPersonAIO.player.position);
        }
        else if (Vector3.Distance(transform.position, FirstPersonAIO.player.position) < playerApproachDistance)
        {
            transform.position = transform.position - transform.forward * 0.1f;
        }
    }

    public void newRoute()
    {
        while (!hasRoute && !attacking)
        {
            RaycastHit hit;
            Vector3 raycastPosition = new Vector3(transform.position.x + Random.Range(-destinationRadius, destinationRadius), 400f, transform.position.z + Random.Range(-destinationRadius, destinationRadius));

            if (Physics.Raycast(raycastPosition, Vector3.up * -1f, out hit, 1000))
            {
                if (hit.transform.gameObject.layer == 9)
                {
                    agent.SetDestination(hit.point);
                    hasRoute = true;
                    waitCoroutine = null;
                }
            }
        }
    }


}

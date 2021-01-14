using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class NPCBehaviour : MonoBehaviour
{
    bool isAlive;
    bool hasRoute;
    NavMeshAgent agent;

    public LayerMask environment;
    public FirstPersonAIO FirstPersonAIO;
    public float inBoundsDistance = 150f;
    public float destinationRadius = 100f;
    
    public float RerouteAtThisDistance = 5f;
    float health;
    float startingHealth = 100;
    

    SkinnedMeshRenderer skinnedMesh;
    Collider[] colliders;
    Rigidbody[] rigidbodies;

    void die()
    {
        isAlive = false;
        agent.isStopped = true;
        agent.enabled = false;
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
        
        FirstPersonAIO = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonAIO>();
        agent = GetComponent<NavMeshAgent>();
        colliders = GetComponentsInChildren<Collider>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        skinnedMesh = GetComponentInChildren<SkinnedMeshRenderer>();
        
        
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

    public void Update()
    {

        if (isAlive && agent.isOnNavMesh)
        {
            if (agent.remainingDistance < RerouteAtThisDistance && !hasRoute)
            {
                newRoute();
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


    }

    public void newRoute()
    {
        while (!hasRoute)
        {
            RaycastHit hit;
            Vector3 raycastPosition = new Vector3(transform.position.x + Random.Range(-destinationRadius, destinationRadius), 400f, transform.position.z + Random.Range(-destinationRadius, destinationRadius));

            if (Physics.Raycast(raycastPosition, Vector3.up * -1f, out hit, 1000))
            {
                if (hit.transform.gameObject.layer == 9)
                {
                    agent.SetDestination(hit.point);
                    hasRoute = true;
                }
            }
        }
    }


}

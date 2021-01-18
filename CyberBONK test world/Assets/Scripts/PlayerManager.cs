using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject playerCamera;
    public GameObject deathCamera;
    public Rigidbody rb;
    public Animator animator;

    public static float health;
    public static float startHealth;

    public Animator bleedingAnimator;
    public static GameObject playerCameraObject;

    private void Start()
    {
        health = 500f;
        startHealth = health;
        playerCameraObject = GetComponentInChildren<Camera>().gameObject;
        playerCamera.SetActive(true);
        deathCamera.SetActive(false);
        //StartCoroutine(delaySpawn());
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {

        if (health <= 0f)
        {
            death();
        }
        else
        {
            if (transform.position.y < -10)
            {
                transform.position = new Vector3(transform.position.x, 4, transform.position.z);
            }


            if (Mathf.Abs(rb.velocity.z) > 0.1f && Mathf.Abs(rb.velocity.x) > 0.1f)
            {
                animator.SetBool("isWalking", true);
            }
            else
            {
                animator.SetBool("isWalking", false);
            }
        }

    }

    public void death()
    {
        GetComponent<Rigidbody>().freezeRotation = false;
        GetComponent<Rigidbody>().AddForce(-transform.forward * 10);
        GetComponent<FirstPersonAIO>().enabled = false;
        animator.SetBool("isWalking", false);

        StartCoroutine(waitThenDeathScreen());
    }

    IEnumerator waitThenDeathScreen()
    {
        yield return new WaitForSeconds(2);
        //playerCamera.SetActive(false);
        GetComponent<Rigidbody>().freezeRotation = true;
        deathCamera.SetActive(true);

    }
    public void spawnNPCs()
    {
        //NPCSpawner.SetActive(true);
    }

    public void takeDamage()
    {
        bleedingAnimator.SetBool("TakenDamage", true);
        health -= 5;
        print(health);
    }

    IEnumerator delaySpawn()
    {
        while (true)
        {
            //NPCSpawner.SetActive(false);
            yield return new WaitForSeconds(10);
            spawnNPCs();
        }
        yield break;
    }
}

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

    public static GameObject player;
    public Animator bleedingAnimator;
    public static GameObject playerCameraObject;

    private void Awake() {
        player = this.gameObject;
    }
    private void Start()
    {
        NPCBehaviour.attacking = false;
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
        //GetComponent<Rigidbody>().AddForce(-transform.forward * 10);
        GetComponent<FirstPersonAIO>().enabled = false;
        animator.SetBool("isWalking", false);
        GetComponent<Animator>().enabled = true;
        StartCoroutine(waitThenDeathScreen());
        //playerCamera.GetComponent<AudioListener>().enabled = false;
    }

    IEnumerator waitThenDeathScreen()
    {
        yield return new WaitForSeconds(2);
        //playerCamera.SetActive(false);
        GetComponent<Rigidbody>().freezeRotation = true;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        deathCamera.SetActive(true);

    }

    public void takeDamage()
    {
        bleedingAnimator.SetBool("TakenDamage", true);
        health -= 5;
    }
}

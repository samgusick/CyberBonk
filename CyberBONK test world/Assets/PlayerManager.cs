using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{    public Rigidbody rb;
    public Animator animator;

    private void Start() {
        //StartCoroutine(delaySpawn());
        rb = GetComponent<Rigidbody>();
    }

    private void Update() {

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
    public void spawnNPCs()
    {
        //NPCSpawner.SetActive(true);
    }

    IEnumerator delaySpawn()
    {
        while(true)
        {
            //NPCSpawner.SetActive(false);
            yield return new WaitForSeconds(10);
            spawnNPCs();
        }
        yield break;
    }
}

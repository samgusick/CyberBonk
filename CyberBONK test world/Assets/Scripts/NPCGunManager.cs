using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCGunManager : MonoBehaviour
{
    public GameObject gunEnd;
    public float timeBetweenShots;
    
    public ParticleSystem gunTrail;
    bool isLookingAtPlayer;
    public LayerMask layer;
    public Transform NPCReference;

    bool firingGun;

    private void Awake() {
        firingGun = false;
    }

    private void Start() {
        StopAllCoroutines();
    }

    private void Update() {
        if (NPCBehaviour.attacking && !firingGun && NPCReference.GetComponent<NPCBehaviour>().isAlive)
        {
            StartCoroutine(firing());
            firingGun = true;
        }

        //Debug.DrawRay(gunEnd.transform.position, PlayerManager.playerCameraObject.transform.position + -0.5f * Vector3.up, Color.red, Time.deltaTime);
    }

    IEnumerator firing()
    {
        while (NPCBehaviour.attacking && NPCReference.GetComponent<NPCBehaviour>().isAlive && PlayerManager.health > 0)
        {
            if (!GetComponentInParent<NPCBehaviour>().agent.isStopped)
            {
                gunEnd.GetComponent<AudioSource>().time = 0;
            RaycastHit hit;
            gunEnd.GetComponent<ParticleSystem>().Play();
            gunEnd.GetComponent<AudioSource>().Play();
            gunTrail.Play();
            
            if (Physics.Raycast(gunEnd.transform.position, NPCReference.forward, out hit ,50, layer))
            {
                //Debug.DrawLine(gunEnd.transform.position, hit.point, Color.blue, 2f);
                if (hit.collider.gameObject.layer == 7)
                {
                    FirstPersonAIO.player.gameObject.GetComponent<PlayerManager>().takeDamage();
                }
            }

            float timeOffset = Random.Range(-2f, 2f);
            yield return new WaitForSeconds(timeBetweenShots + timeOffset);
            }
            else
            {
                yield return null;
            }
        
        }

        firingGun = false;
        yield break;
    
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    // Start is called before the first frame update

    FirstPersonAIO playerMoveReference;
    Animator animator;
    public AudioClip[] playerAudioClips;
    new AudioSource audio;

    void Start()
    {
        playerMoveReference = GetComponentInChildren<FirstPersonAIO>();
        animator = GetComponentInChildren<Animator>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetBool("isWalking"))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                audio.clip = playerAudioClips[1];
            }
            else
            {
                audio.clip = playerAudioClips[0];
            }

            if (!audio.isPlaying)
            {
                audio.Play();
            }
        }
        
        else
        {
            audio.Stop();
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
public class deathCameraManager : MonoBehaviour
{
    public VideoPlayer video;

    public GameObject arms;
    public GameObject healthbar;
    public GameObject minimap;
    public GameObject crossHair;
    public Animator animator;

    public Animator fadeFromBlack;
    public AudioSource audio;

    private void Awake()
    {
        StartCoroutine(waitThenSurprise());   
    }

    IEnumerator waitThenSurprise()
    {
        video.enabled = true;
        video.Play();
        yield return new WaitForSeconds(4);
        healthbar.SetActive(false);
        minimap.SetActive(false);
        crossHair.SetActive(false);
        fadeFromBlack.gameObject.SetActive(true);
        arms.SetActive(false);
        fadeFromBlack.SetBool("isFading", true);
        yield return new WaitForSeconds(2);
        animator.SetBool("deathScreen", true);
        audio.Play();
        yield return new WaitForSeconds(audio.clip.length);
        SceneManager.LoadScene(0);
        yield break;
    }
    private void Update()
    {
        if (video.isPlaying && video.time > 4)
        {
            video.Stop();
        }
    }
}

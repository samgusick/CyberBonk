using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static bool musicOn;

    public static float startAt;
    public static new AudioSource audio;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.M))
        {
            musicOn = !musicOn;
        }

        if (musicOn)
        {
            if (!audio.isPlaying)
            {
                playMusic();
            }
            
        }
        else
        {
            pauseMusic();
        }

        if (PlayerManager.health <= 0)
        {
            stopMusic();
        }
    }
    private void Start() {
        audio = GetComponent<AudioSource>();
        startAt = Random.Range(0f, audio.clip.length);
        audio.time = startAt;
        playMusic();
        musicOn = true;
    }

    public static void playMusic()
    {
        audio.Play();
    }

    public static void pauseMusic()
    {
        audio.Pause();
    }

    public static void unpauseMusic()
    {
        audio.UnPause();
    }

    public static void stopMusic()
    {
        audio.Stop();
    }
}

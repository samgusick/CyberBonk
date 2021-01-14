using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static new AudioSource audio;
    private void Start() {
        audio = GetComponent<AudioSource>();
        playMusic();
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

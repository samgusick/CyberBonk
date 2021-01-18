using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAudioManager : MonoBehaviour
{
    
    public AudioClip[] KeanuClips;
    public new AudioSource audio;
    public NPCTextHandling textHandling;

    int clipNumber;

    public void playClip()
    {
        audio.clip = getRandomAudioClip();
        StartCoroutine(stopMusic(audio.clip.length));
        audio.Play();
    }

    private void Update()
    {
        if (audio.isPlaying)
        {
            sendClipText(clipNumber);
        }
        else
        {
            textHandling.setTextNull();
        }

    }

    public AudioClip getRandomAudioClip()
    {
        AudioClip randomClip;
        clipNumber = Random.Range(0, KeanuClips.Length);
        randomClip = KeanuClips[clipNumber];
        return randomClip;
    }

    IEnumerator stopMusic(float stopDuration)
    {
        float originalVolume = MusicManager.audio.volume;
        MusicManager.audio.volume = originalVolume/4f;
        yield return new WaitForSeconds(stopDuration);
        MusicManager.audio.volume = originalVolume;
        yield break;
    }

    public void sendClipText(int clipNumber)
    {
        switch (clipNumber)
        {

            default:
                textHandling.setText("Cock");
                break;

            case (0):
                textHandling.setText("Getting C*ck");
                break;

            case (1):
                textHandling.setText("Welcome to C*m C*ck City");
                break;

            case (2):
                textHandling.setText("Why do you C*m?");
                break;

            case (3):
                textHandling.setText("Dick");
                break;

            case (4):
                textHandling.setText("Well, C*m");
                break;

            case (5):
                textHandling.setText("You're breathtaking!");
                break;

            case (6):
                textHandling.setText("Wake the f*ck up, samurai. We have a city to burn");
                break;




        }
    }
}

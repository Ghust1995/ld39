using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxManager : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip clickClip;
    public AudioClip tweetClip;
    public AudioClip evolutionClip;
    public AudioClip victoryClip;
    public AudioClip chargingClip;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            audioSource.clip = clickClip;
            audioSource.Play();
        }
    }

    public void PlayTweet()
    {
        audioSource.clip = tweetClip;
        audioSource.Play();
    }

    public void PlayEvolution()
    {
        audioSource.clip = evolutionClip;
        audioSource.Play();
    }
    public void PlayCapture()
    {
        audioSource.clip = victoryClip;
        audioSource.Play();
    }
    public void PlayCharging()
    {
        audioSource.clip = chargingClip;
        audioSource.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Noise : MonoBehaviour
{
    [SerializeField] AudioClip noiseLoop;
    [SerializeField] AudioClip noiseEnd;

    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayEndNoiseSound()
    {
        audioSource.loop = false;
        audioSource.clip = noiseEnd;
        audioSource.Play();
    }

    public void PlayLoopSound()
    {
        audioSource.loop = true;
        audioSource.clip = noiseLoop;
        audioSource.Play();
    }
}

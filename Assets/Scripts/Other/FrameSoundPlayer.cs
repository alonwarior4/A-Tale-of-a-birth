using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameSoundPlayer : MonoBehaviour
{
    AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayFrameSound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip, 1f);
    }
}

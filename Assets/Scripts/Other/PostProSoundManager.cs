using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PostProSoundManager : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] float[] blendWeights;
    [SerializeField] AudioMixerSnapshot[] snapshots;
    [SerializeField] AudioClip postProAudio;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayPPSound()
    {
        audioSource.PlayOneShot(postProAudio,1f);
    }

    public void SilentBackground()
    {
        blendWeights[0] = 1; // index 0 = Post pro
        blendWeights[1] = 0; // index 1 = Background
        audioMixer.TransitionToSnapshots(snapshots, blendWeights, 0.2f);
    }

    public void TurnBackBackground()
    {
        blendWeights[0] = 0;
        blendWeights[1] = 1;
        audioMixer.TransitionToSnapshots(snapshots, blendWeights, 2f);
    }

    public void SetSecondSnapShot(AudioMixerSnapshot newSnapshot)
    {
        snapshots[0] = newSnapshot;
    }
    public void SetPostProAudio(AudioClip ppAudio)
    {
        postProAudio = ppAudio;
    }
}

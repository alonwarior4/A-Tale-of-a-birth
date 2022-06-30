using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;

public class BGMusic_Controller : MonoBehaviour
{
    [Header("Sound Manager")]
    [SerializeField] AudioMixer bgMixer;
    public AudioMixer masterMix;
    [SerializeField] AudioMixerSnapshot[] snapshots;
    public AudioSource[] audioSources;
    [SerializeField] float[] blendWeights;

    [Header("Sound Toggle Button Image Manager")]
    [SerializeField] Image soundImage;
    [SerializeField] Sprite soundOn;
    [SerializeField] Sprite soundOff;

    public AudioMixerSnapshot[] masterSnapShots;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("Sound"))
        {
            PlayerPrefs.SetInt("Sound", 1);
            soundImage.sprite = soundOn;
            masterMix.SetFloat("MasterVolume", 0);
        }
        else
        {
            if (PlayerPrefs.GetInt("Sound") == 1)
            {
                soundImage.sprite = soundOn;
                masterMix.SetFloat("MasterVolume", 0);
            }
            else
            {
                soundImage.sprite = soundOff;
                masterMix.SetFloat("MasterVolume", -80);
            }
        }
    }

    public void BlendSectionBGs(int firstSoundIndex , int LastSoundIndex , float SnapshotValue)
    {
        audioSources[firstSoundIndex].enabled = false;
        audioSources[LastSoundIndex].enabled = true;
        blendWeights[firstSoundIndex] = 0f;
        blendWeights[LastSoundIndex] = 1f;
        bgMixer.TransitionToSnapshots(snapshots, blendWeights, SnapshotValue);
    }

    public void SoundManager()
    {
        if(PlayerPrefs.GetInt("Sound") == 1)
        {
            PlayerPrefs.SetInt("Sound", 0);
            soundImage.sprite = soundOff;
            masterMix.SetFloat("MasterVolume", -80);
        }
        else
        {
            PlayerPrefs.SetInt("Sound", 1);
            soundImage.sprite = soundOn;
            masterMix.SetFloat("MasterVolume", 0);
        }
    }
}

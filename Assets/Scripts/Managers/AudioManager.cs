using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private AudioSource audioSource;

    [SerializeField] private AudioClip audioHitTheBox;
    [SerializeField] private AudioClip audioHitOtherBall;
    [SerializeField] private AudioClip audioCombine;
    [SerializeField] private AudioClip audioGameOver;

    private float sfxVolume;


    // Start is called before the first frame update
    void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(this);
        }

        GameManager.Instance.OnSettingsChanged += SetVolume;
        SetVolume();
        audioSource = GetComponent<AudioSource>();
    }

    private void SetVolume()
    {
        sfxVolume = GameManager.Instance.SfxVolume / 100;

    }

    public void PlayCombineAudio()
    {
        audioSource.volume = sfxVolume;
        audioSource.PlayOneShot(audioCombine);
    }

    public void PlayBoxAudio()
    {
        audioSource.volume = sfxVolume / 3;
        audioSource.PlayOneShot(audioHitTheBox);
    }

    public void PlayGameOverAudio()
    {
        audioSource.volume = sfxVolume / 3;
        audioSource.PlayOneShot(audioGameOver);
    }

    public void PlayHitOtherBallAudio()
    {
        audioSource.volume = sfxVolume / 3;
        audioSource.PlayOneShot(audioHitOtherBall);
    }
}

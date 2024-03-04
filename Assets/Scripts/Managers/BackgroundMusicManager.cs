using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetMusicVolume();
        GameManager.Instance.OnSettingsChanged += SetMusicVolume;
    }

    private void SetMusicVolume()
    {
        _audioSource.volume = GameManager.Instance.MusicVolume / 100;
    }
}

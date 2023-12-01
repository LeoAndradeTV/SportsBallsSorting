using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneSliders : MonoBehaviour
{
    [SerializeField] private Slider movementSlider;
    [SerializeField] private Slider cameraSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    // Start is called before the first frame update
    void OnEnable()
    {
        movementSlider.value = GameManager.Instance.MovementSpeed;
        cameraSlider.value = GameManager.Instance.CameraSpeed;
        musicVolumeSlider.value = GameManager.Instance.MusicVolume;
        sfxVolumeSlider.value = GameManager.Instance.SfxVolume;

        movementSlider.onValueChanged.AddListener(GameManager.Instance.SetMovementSpeed);
        cameraSlider.onValueChanged.AddListener(GameManager.Instance.SetCameraSpeed);
        musicVolumeSlider.onValueChanged.AddListener(GameManager.Instance.SetMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(GameManager.Instance.SetSFXVolume);
    }

    // Update is called once per frame
    void OnDisable()
    {
        movementSlider.onValueChanged.RemoveListener(GameManager.Instance.SetMovementSpeed);
        cameraSlider.onValueChanged.RemoveListener(GameManager.Instance.SetCameraSpeed);
        musicVolumeSlider.onValueChanged.RemoveListener(GameManager.Instance.SetMusicVolume);
        sfxVolumeSlider.onValueChanged.RemoveListener(GameManager.Instance.SetSFXVolume);
    }
}

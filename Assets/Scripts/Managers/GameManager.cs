using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using JetBrains.Annotations;
using System.IO;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool UserHasPlayedGame { get; private set; }

    public static GameManager Instance { get; private set; }

    public Action OnSettingsChanged;
    public Action OnGameOver;
    public Action OnTutorialClosed;

    public GameState CurrentState { get; private set; }

    public float MovementSpeed { get; private set; }
    public float CameraSpeed { get; private set; }
    public float MusicVolume { get; private set; }
    public float SfxVolume { get; private set; }

    private IDataService dataService = new JsonDataService();

    private void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        else
        {
            Destroy(gameObject);
        }

        LoadMySettings();
        OnTutorialClosed += CloseTutorialStartGame;
        CurrentState = GameState.Playing;
    }

    private void InitDefaultSettings()
    {
        SetMovementSpeed(80f);
        SetCameraSpeed(20f);
        SetMusicVolume(50f);
        SetSFXVolume(50f);
    }

    public void LoadGameScene()
    {
        CurrentState = GameState.Playing;
        SceneManager.LoadScene(SceneTypes.GameScene.ToString());
#if UNITY_IOS
    SceneManager.LoadScene(SceneTypes.MobileGameScene.ToString());
#endif
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene(SceneTypes.MainMenu.ToString());
    }

    public void SaveMySettings()
    {
        SaveSettings settings = new SaveSettings();
        settings.handSpeed = MovementSpeed;
        settings.cameraSpeed = CameraSpeed;
        settings.musicVolume = MusicVolume;
        settings.sfxVolume = SfxVolume;
        settings.userHasPlayedGame = UserHasPlayedGame;
        //string data = JsonUtility.ToJson(settings);

        //PlayerPrefs.SetString("settings", data);
        //PlayerPrefs.Save();

        if (dataService.SaveData("/player-settings.json", settings, false))
        {
            
        } else
        {
            
        }
    }

    public void LoadMySettings()
    {
        //string data = PlayerPrefs.GetString("settings");

        SaveSettings settings = dataService.LoadData<SaveSettings>("/player-settings.json", false);

        MovementSpeed = settings.handSpeed;
        CameraSpeed = settings.cameraSpeed;
        MusicVolume = settings.musicVolume;
        SfxVolume = settings.sfxVolume;
        UserHasPlayedGame = settings.userHasPlayedGame;
    }

    public void CloseTutorialStartGame()
    {
        UserHasPlayedGame = true;
        SaveMySettings();
        CurrentState = GameState.Playing;
    }

    public void PauseGame()
    {
        CurrentState = GameState.Paused;
    }

    public void CloseGame()
    {
        SaveMySettings();
        Application.Quit();
    }

    public void ResumeGame()
    {
        CurrentState = GameState.Playing;
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        ScoreManager.Instance.SaveHighScore();
        CurrentState = GameState.Paused;
        Time.timeScale = 0;
        OnGameOver?.Invoke();
    }

    public void SetMovementSpeed(float speed)
    {
        MovementSpeed = speed;
        OnSettingsChanged?.Invoke();
    }

    public void SetCameraSpeed(float speed)
    {
        CameraSpeed = speed;
        OnSettingsChanged?.Invoke();

    }

    public void SetMusicVolume(float volume)
    {
        MusicVolume = volume;
        OnSettingsChanged?.Invoke();

    }

    public void SetSFXVolume(float volume)
    {
        SfxVolume = volume;
        OnSettingsChanged?.Invoke();

    }
}

public enum GameState
{
    Playing,
    Tutorial,
    Paused
}

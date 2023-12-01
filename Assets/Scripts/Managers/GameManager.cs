using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Action OnSettingsChanged;
    public Action OnGameOver;

    public GameState CurrentState { get; private set; }

    public float MovementSpeed { get; private set; }
    public float CameraSpeed { get; private set; }
    public float MusicVolume { get; private set; }
    public float SfxVolume { get; private set; }

    private void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        else
        {
            Destroy(Instance);
        }

        InitDefaultSettings();

        CurrentState = GameState.Playing;
    }

    private void InitDefaultSettings()
    {
        SetMovementSpeed(50f);
        SetCameraSpeed(50f);
        SetMusicVolume(50f);
        SetSFXVolume(50f);
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(SceneTypes.GameScene.ToString());
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene(SceneTypes.MainMenu.ToString());
    }

    public void PauseGame()
    {
        CurrentState = GameState.Paused;
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void ResumeGame()
    {
        CurrentState = GameState.Playing;
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
        ScoreManager.Instance.SerializeJson();
        CurrentState = GameState.Paused;
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
    Paused
}

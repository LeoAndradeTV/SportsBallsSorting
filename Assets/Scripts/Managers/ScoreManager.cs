using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highscoreText;

    private int score;
    private IDataService dataService = new JsonDataService();
    private int highScore;

    public int HighScore
    {
        get { return highScore; }
        set 
        { 
            highScore = value;
            highscoreText.text = highScore.ToString();
        }
    }

    public int Score
    {
        get { return score; }
        set 
        { 
            score = value; 
            scoreText.text = score.ToString();
        }
    }

    private void OnEnable()
    {
        if (Instance == null)
            Instance = this;

        Score = 0;
        DeserializeJson();
    }

    public void SerializeJson()
    {
        if (Score < HighScore) { return; }

        if (dataService.SaveData("/player-highscore.json", Score, false))
        {

        } else
        {
            Debug.LogError("Data failed to save");
        }
    }

    public void DeserializeJson()
    {
        HighScore = dataService.LoadData<int>("/player-highscore.json", false);
    }
}

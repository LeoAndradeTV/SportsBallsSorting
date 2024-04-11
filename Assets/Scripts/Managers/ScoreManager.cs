using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highscoreText;

    [SerializeField] private int score;
    private int highScore;

    private CallResult<LeaderboardFindResult_t> leaderboardResult;
    SteamLeaderboard_t leaderboardHandle;

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
            if (Score > HighScore)
            {
                SaveHighScore();
            }
        }
    }

    private void OnEnable()
    {
        if (SteamManager.Initialized)
        {
            leaderboardResult = CallResult<LeaderboardFindResult_t>.Create(OnFindLeaderboard);
            var leaderboard = SteamUserStats.FindLeaderboard("Ball Blitz Highscore");
            leaderboardResult.Set(leaderboard);
        }
    }

    private void OnFindLeaderboard(LeaderboardFindResult_t param, bool bIOFailure)
    {
        if (param.m_bLeaderboardFound != 1 || bIOFailure)
        {
            
        } else
        {
            leaderboardHandle = param.m_hSteamLeaderboard;
        }

    }

    private void Start()
    {
        if (Instance == null)
            Instance = this;

        Score = 0;
        LoadHighScore();

        BallCombineManager.Instance.OnBallCombined += AddToScore;
    }

    private void AddToScore(object sender, BallCombineManager.BallToCombineEventArgs e)
    {
        Score += e.ballToCombine.GetBallPoints();
    }

    public void SaveHighScore()
    {
        if (Score < HighScore) { return; }

        if (!SteamManager.Initialized) { return; }

        SteamUserStats.SetStat("HIGHSCORE", Score);
        SteamUserStats.StoreStats();

        LoadHighScore();

         SteamUserStats.UploadLeaderboardScore(leaderboardHandle, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodForceUpdate, HighScore, new int[0], 0);
        
    }

    public void LoadHighScore()
    {
        if (!SteamManager.Initialized) { return; }

        SteamUserStats.GetStat("HIGHSCORE", out int highscore);
        HighScore = highscore;
    }
}

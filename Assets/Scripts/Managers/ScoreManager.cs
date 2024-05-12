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
#if UNITY_STANDALONE
    private CallResult<LeaderboardFindResult_t> leaderboardResult;
    SteamLeaderboard_t leaderboardHandle;
#endif

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
            if (score > HighScore)
            {
                HighScore = score;
                SaveHighScore();
            }
        }
    }
#if UNITY_STANDALONE

    private void OnEnable()
    {
        if (SteamManager.Initialized)
        {
            var leaderboard = SteamUserStats.FindLeaderboard("Ball Blitz Highscore");

            leaderboardResult = CallResult<LeaderboardFindResult_t>.Create(OnFindLeaderboard);
            leaderboardResult.Set(leaderboard);
        }
    }

    private void OnFindLeaderboard(LeaderboardFindResult_t param, bool bIOFailure)
    {
        if (param.m_bLeaderboardFound != 1 || bIOFailure)
        {

        }
        else
        {
            leaderboardHandle = param.m_hSteamLeaderboard;
            UploadToLeaderboard();
        }

    }
#endif

    private void Start()
    {
        if (Instance == null)
            Instance = this;

        Score = 0;

        //For Debugging Only
        //ResetMyHighscore();

        LoadHighScore();
        Debug.Log(HighScore);

        BallCombineManager.Instance.OnBallCombined += AddToScore;
    }

    private void OnDisable()
    {
        BallCombineManager.Instance.OnBallCombined -= AddToScore;
    }

    private void AddToScore(object sender, BallCombineManager.BallToCombineEventArgs e)
    {
        Score += e.ballToCombine.GetBallPoints();
    }


    public void SaveHighScore()
    {
#if UNITY_STANDALONE
        if (!SteamManager.Initialized) { return; }

        // Update the local high score if the current score is higher

        SteamUserStats.SetStat("HIGHSCORE", Score);
        SteamUserStats.StoreStats();
#endif

    }

    public void UploadToLeaderboard()
    {
#if UNITY_STANDALONE

        if (!SteamManager.Initialized) { return; }

        SteamUserStats.UploadLeaderboardScore(leaderboardHandle, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodForceUpdate, LoadHighScore(), new int[0], 0);
#endif
    }

    public int LoadHighScore()
    {
#if UNITY_STANDALONE

        if (!SteamManager.Initialized) { return 0; }

        SteamUserStats.GetStat("HIGHSCORE", out int highscore);
        HighScore = highscore;
#endif
        return highScore;
    }

    // For Debuggin Only
    private void ResetMyHighscore()
    {
#if UNITY_STANDALONE

        if (!SteamManager.Initialized) { return; }

        SteamUserStats.SetStat("HIGHSCORE", 0);
        SteamUserStats.StoreStats();
#endif
    }
}

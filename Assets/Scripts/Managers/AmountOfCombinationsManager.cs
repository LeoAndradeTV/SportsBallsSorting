using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class AmountOfCombinationsManager : MonoBehaviour
{
    private Dictionary<BallData, int> amountOfBallsCombined;

    // Start is called before the first frame update
    void Awake()
    {
        amountOfBallsCombined = new Dictionary<BallData, int>();
    }

    private void Start()
    {
        BallCombineManager.Instance.OnBallCombined += Instance_OnBallCombined;
    }
    
    private void Instance_OnBallCombined(object sender, BallCombineManager.BallToCombineEventArgs e)
    {
        Ball ball = e.ballToCombine.GetNextBall();
        BallData ballData = ball.GetBallData();
        if (amountOfBallsCombined.ContainsKey(ballData))
        {
            amountOfBallsCombined[ballData]++;
        }
        else
        {
            amountOfBallsCombined[ballData] = 1;
        }
#if UNITY_STANDALONE
        if (SteamManager.Initialized)
        {
            SetCombinationValueToServer(ball);
            TriggerFirstBallAchievement(ball);
        }
#endif

    }
#if UNITY_STANDALONE
    private void SetCombinationValueToServer(Ball ball)
    {
        BallType ballType = ball.GetBallData().ballType;
        int ballCount = amountOfBallsCombined[ball.GetBallData()];

        switch (ballType)
        {
            case BallType.Volleyball:
                SteamUserStats.GetStat("VOLLEYBALL_COMBINED", out ballCount);
                ballCount++;
                SteamUserStats.SetStat("VOLLEYBALL_COMBINED", ballCount);
                SteamUserStats.StoreStats();
                break;
            case BallType.SoccerBall:
                SteamUserStats.GetStat("SOCCER_BALL_COMBINED", out ballCount);
                ballCount++;
                SteamUserStats.SetStat("SOCCER_BALL_COMBINED", ballCount);
                SteamUserStats.StoreStats();
                break;
            case BallType.BowlingBall:
                SteamUserStats.GetStat("BOWLING_BALL_COMBINED", out ballCount);
                ballCount++;
                SteamUserStats.SetStat("BOWLING_BALL_COMBINED", ballCount);
                SteamUserStats.StoreStats();
                break;
            case BallType.Football:
                SteamUserStats.GetStat("FOOTBALL_COMBINED", out ballCount);
                ballCount++;
                SteamUserStats.SetStat("FOOTBALL_COMBINED", ballCount);
                SteamUserStats.StoreStats();
                break;
            case BallType.Basketball:
                SteamUserStats.GetStat("BASKETBALL_COMBINED", out ballCount);
                ballCount++;
                SteamUserStats.SetStat("BASKETBALL_COMBINED", ballCount);
                SteamUserStats.StoreStats();
                break;
            case BallType.BeachBall:
                SteamUserStats.GetStat("BEACH_BALL_COMBINED", out ballCount);
                ballCount++;
                SteamUserStats.SetStat("BEACH_BALL_COMBINED", ballCount);
                SteamUserStats.StoreStats();
                break;
        }
    }

    private void TriggerFirstBallAchievement(Ball ball)
    {
        BallType type = ball.GetBallData().ballType;

        if (amountOfBallsCombined[ball.GetBallData()] >= 1)
        {
            switch (type)
            {
                case BallType.Volleyball:
                    Steamworks.SteamUserStats.GetAchievement("FIRST_VOLLEYBALL", out bool firstVolleyballCompleted);
                    if (!firstVolleyballCompleted)
                    {
                        SteamUserStats.SetAchievement("FIRST_VOLLEYBALL");
                        SteamUserStats.StoreStats();
                    }
                    break;
                case BallType.SoccerBall:
                    Steamworks.SteamUserStats.GetAchievement("FIRST_SOCCER_BALL", out bool firstSoccerBallCompleted);
                    if (!firstSoccerBallCompleted)
                    {
                        SteamUserStats.SetAchievement("FIRST_SOCCER_BALL");
                        SteamUserStats.StoreStats();
                    }
                    break;
                case BallType.BowlingBall:
                    Steamworks.SteamUserStats.GetAchievement("FIRST_BOWLING_BALL", out bool firstBowlingBallCompleted);
                    if (!firstBowlingBallCompleted)
                    {
                        SteamUserStats.SetAchievement("FIRST_BOWLING_BALL");
                        SteamUserStats.StoreStats();
                    }
                    break;
                case BallType.Football:
                    Steamworks.SteamUserStats.GetAchievement("FIRST_FOOTBALL", out bool firstFootballCompleted);
                    if (!firstFootballCompleted)
                    {
                        SteamUserStats.SetAchievement("FIRST_FOOTBALL");
                        SteamUserStats.StoreStats();
                    }
                    break;
                case BallType.Basketball:
                    Steamworks.SteamUserStats.GetAchievement("FIRST_BASKETBALL", out bool firstBasketballCompleted);
                    if (!firstBasketballCompleted)
                    {;
                        SteamUserStats.SetAchievement("FIRST_BASKETBALL");
                        SteamUserStats.StoreStats();
                    }
                    break;
                case BallType.BeachBall:
                    Steamworks.SteamUserStats.GetAchievement("FIRST_BEACH_BALL", out bool firstBeachBallCompleted);
                    if (!firstBeachBallCompleted)
                    {
                        SteamUserStats.SetAchievement("FIRST_BEACH_BALL");
                        SteamUserStats.StoreStats();
                    }
                    break;
            }
        }

    }
#endif


}

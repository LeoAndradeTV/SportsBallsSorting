using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCombineManager : MonoBehaviour
{
    public static BallCombineManager Instance { get; private set; }

    private List<Ball> ballsToCombine = new List<Ball>();

    // Start is called before the first frame update
    void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void AddToBallsList(Ball ballToAdd)
    {
        ballsToCombine.Add(ballToAdd);
    }

    public void Combine(Vector3 combinePosition)
    {
        if (ballsToCombine.Count == 2)
        {
            Instantiate(ballsToCombine[0].GetNextBall(), combinePosition, Quaternion.identity);
            ScoreManager.Instance.Score += ballsToCombine[0].GetBallPoints();
            ballsToCombine.Clear();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCombineManager : MonoBehaviour
{
    public static BallCombineManager Instance { get; private set; }

    private List<Ball> ballsToCombine = new List<Ball>();

    private bool hasRecentlyCombined;

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

    public void PlayCombineEffect(ParticleSystem ballEffect, Vector3 position, Ball ball)
    {
        GameObject effect = Instantiate(ballEffect.gameObject, position, Quaternion.identity);    
    }

    public void Combine(Vector3 combinePosition)
    {
        if (ballsToCombine.Count == 2)
        {
            if (hasRecentlyCombined)
            {
                ballsToCombine.Clear();
                return;
            }

            Ball ball = Instantiate(ballsToCombine[0].GetNextBall(), combinePosition, Quaternion.identity);
            ball.SetHasCollided();
            ScoreManager.Instance.Score += ballsToCombine[0].GetBallPoints();
            PlayCombineEffect(ball.GetEffect(), combinePosition, ball);
            AudioManager.Instance.PlayCombineAudio();
            foreach (Ball b in ballsToCombine)
            {
                Destroy(b.gameObject);
            }
            ballsToCombine.Clear();
            hasRecentlyCombined = true;
            ResetRecentCombination();
        }
    }

    private async void ResetRecentCombination()
    {
        await System.Threading.Tasks.Task.Delay(5);
        hasRecentlyCombined = false;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCombineManager : MonoBehaviour
{
    public event EventHandler<BallToCombineEventArgs> OnBallCombined;
    public class BallToCombineEventArgs : EventArgs
    {
        public Ball ballToCombine;
        public Vector3 popUpLocation;
    }

    public static BallCombineManager Instance { get; private set; }

    private List<Ball> ballsToCombine = new List<Ball>();

    private bool hasRecentlyCombined;

    // Start is called before the first frame update
    void Awake()
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

            float pushDownFactor = combinePosition.y - ballsToCombine[0].GetNextBall().GetCurrentBallRadius().y < ballsToCombine[0].GetNextBall().GetCurrentBallRadius().y ? 0 : ballsToCombine[0].GetNextBall().GetCurrentBallRadius().y;

            Ball ball = Instantiate(ballsToCombine[0].GetNextBall(), combinePosition - (Vector3.up * pushDownFactor), Quaternion.identity);
            ball.SetHasCollided();
            PlayCombineEffect(ball.GetEffect(), combinePosition, ball);
            Vector3 screenCombinePosition = Camera.main.WorldToScreenPoint(combinePosition);
            OnBallCombined?.Invoke(this, new BallToCombineEventArgs
            {
                ballToCombine = ballsToCombine[0],
                popUpLocation = screenCombinePosition
            }) ;
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
        await System.Threading.Tasks.Task.Delay(1);
        hasRecentlyCombined = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private BallData _ballData;

    private int _pointsAtCombining;
    private Ball _nextBall;
    private BallType _ballType;

    // Start is called before the first frame update
    void OnEnable()
    {
        Initialize();
    }

    private void Initialize()
    {
        _pointsAtCombining = _ballData.pointsAtCombining;
        _nextBall = _ballData.nextBall;
        _ballType = _ballData.ballType;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Floor"))
        {
            GameManager.Instance.GameOver();
        }

        Ball collidingBall = collision.gameObject.GetComponent<Ball>();

        if (collidingBall == null) return;

        if (collidingBall._ballType != _ballType) return;

        BallCombineManager.Instance.AddToBallsList(this);
        BallCombineManager.Instance.Combine(collision.contacts[0].point);

        Destroy(gameObject);
    }

    public Ball GetNextBall()
    {
        return _nextBall;
    }

    public int GetBallPoints()
    {
        return _pointsAtCombining;
    }
}

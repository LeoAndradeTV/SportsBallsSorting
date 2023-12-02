using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private BallData _ballData;

    private Sprite _ballSprite;
    private int _pointsAtCombining;
    private Ball _nextBall;
    private BallType _ballType;
    private bool hasCollided;

    // Start is called before the first frame update
    void OnEnable()
    {
        Initialize();
    }

    private void Update()
    {
        if (transform.parent != null)
        {
            transform.localPosition = Vector3.zero;
        }
    }

    private void Initialize()
    {
        _pointsAtCombining = _ballData.pointsAtCombining;
        _nextBall = _ballData.nextBall;
        _ballType = _ballData.ballType;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasCollided)
        {
            ActionsManager.BallHasCollided?.Invoke();
            SetHasCollided();
        }

        if (collision.collider.CompareTag("Floor"))
        {
            GameManager.Instance.GameOver();
        }

        GetComponent<Rigidbody>().AddForce(GetRandomForce());

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

    public Sprite GetBallSprite()
    {
        _ballSprite = _ballData.ballSprite;
        return _ballSprite;
    }

    public void SetHasCollided()
    {
        hasCollided = true;
    }

    private Vector3 GetRandomForce()
    {
        return new Vector3(Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f));
    }
}

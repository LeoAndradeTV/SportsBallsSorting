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
    private ParticleSystem _particleSystem;

    // Start is called before the first frame update
    void OnEnable()
    {
        Initialize();
    }

    private void Update()
    {
        if (transform.parent != null)
        {
            transform.position = transform.parent.position;
        }
    }

    private void Initialize()
    {
        _pointsAtCombining = _ballData.pointsAtCombining;
        _nextBall = _ballData.nextBall;
        _ballType = _ballData.ballType;
        _particleSystem = _ballData.particleSystem;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Ball goes out of the box
        if (collision.collider.CompareTag("Floor"))
        {
            AudioManager.Instance.PlayGameOverAudio();
            GameManager.Instance.GameOver();
            return;
        }

        // Adds tiny random force so balls can't stack
        GetComponent<Rigidbody>().AddForce(GetRandomForce());

        // Grabs a ball that we're colliding with
        Ball collidingBall = collision.gameObject.GetComponent<Ball>();

        // If the first collision isn't a ball
        if (collidingBall == null)
        {
            if (!hasCollided)
            {
                AudioManager.Instance.PlayBoxAudio();
                ActionsManager.BallHasCollided?.Invoke();
                SetHasCollided();
            }
            return;
        }

        // If the first collision is a ball but not of the same type
        if (collidingBall._ballType != _ballType)
        {
            if (!hasCollided)
            {
                AudioManager.Instance.PlayHitOtherBallAudio();
                ActionsManager.BallHasCollided?.Invoke();
                SetHasCollided();
            }
            return;
        }

        if (!hasCollided)
        {
            ActionsManager.BallHasCollided?.Invoke();
            SetHasCollided();
        }

        BallCombineManager.Instance.AddToBallsList(this);
        BallCombineManager.Instance.Combine(collision.contacts[0].point);
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

    public ParticleSystem GetEffect()
    {
        return _particleSystem;
    }

    public void SetHasCollided()
    {
        hasCollided = true;
    }

    public void DropBall()
    {
        transform.position = transform.parent.position;

        ToggleBall(true);

        Debug.Log($"Ball position is: {transform.position}");

        transform.parent = null;

    }

    public void ToggleBall(bool toggle)
    {
        GetComponent<Rigidbody>().useGravity = toggle;
        GetComponent<Collider>().enabled = toggle;
    }

    private Vector3 GetRandomForce()
    {
        return new Vector3(Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f));
    }
}

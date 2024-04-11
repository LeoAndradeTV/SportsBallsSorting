using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private BallData _ballData;

    private Rigidbody _rb;
    private Collider _collider;

    private Sprite _ballSprite;
    private int _pointsAtCombining;
    private Ball _nextBall;
    private BallType _ballType;
    private bool hasCollided;
    private ParticleSystem _particleSystem;
    private Color _ballColor;

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
        _particleSystem = _ballData.particleSystem;
        _ballColor = _ballData.backgroundColor;
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Ball goes out of the box
        if (collision.collider.CompareTag("Floor"))
        {
            AudioManager.Instance.PlayGameOverAudio();
            GameManager.Instance.GameOver();
            Destroy(gameObject);
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

    public BallData GetBallData()
    {
        return _ballData;
    }

    public BallType GetBallType()
    {
        return _ballType;
    }

    public Ball GetNextBall()
    {
        return _nextBall;
    }

    public Color GetNextBallColor()
    {
        return _ballColor;
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
        if (transform.parent == null) { return ; }
        transform.parent = null;
        ToggleBall(true);
    }

    public void ToggleBall(bool toggle)
    {
        _rb.useGravity = toggle;
        _collider.enabled = toggle;
    }

    private Vector3 GetRandomForce()
    {
        return new Vector3(Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f));
    }
}

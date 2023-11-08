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

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Initialize()
    {
        _pointsAtCombining = _ballData.pointsAtCombining;
        _nextBall = _ballData.nextBall;
        _ballType = _ballData.ballType;
    }
}

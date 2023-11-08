using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ball Data", menuName = "Ball Data")]
public class BallData : ScriptableObject
{
    public int pointsAtCombining;
    public Ball nextBall;
    public BallType ballType;

    // TODO:
    // Add audio clip
    // Add particle effect
}

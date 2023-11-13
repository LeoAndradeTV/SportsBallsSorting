using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] private TMP_Text scoreText;

    private int score;

    public int Score
    {
        get { return score; }
        set 
        { 
            score = value; 
            scoreText.text = score.ToString();
        }
    }

    private void OnEnable()
    {
        if (Instance == null)
            Instance = this;

        Score = 0;
    }
}

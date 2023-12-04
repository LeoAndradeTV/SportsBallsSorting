using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private Button replayButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private TMP_Text finalScoreText;

    // Start is called before the first frame update
    void OnEnable()
    {
        replayButton.onClick.AddListener(() =>
        {
            GameManager.Instance.LoadGameScene();
            Hide();
        });

        quitButton.onClick.AddListener(() =>
        {
            GameManager.Instance.LoadMenuScene();
            Hide();
        });

        if (ScoreManager.Instance.Score > ScoreManager.Instance.HighScore)
        {
            finalScoreText.text = $"New High Score!  {ScoreManager.Instance.Score}";
        }
        else
        {
            finalScoreText.text = $"Your Final Score: {ScoreManager.Instance.Score}";
        }

    }

    private void OnDisable()
    {
        replayButton.onClick.RemoveListener(() =>
        {
            GameManager.Instance.LoadGameScene();
            Hide();
        });

        quitButton.onClick.RemoveListener(() =>
        {
            GameManager.Instance.LoadMenuScene();
            Hide();
        });
    }

    private void Hide()
    {
        gameObject.SetActive(false);

    }
}

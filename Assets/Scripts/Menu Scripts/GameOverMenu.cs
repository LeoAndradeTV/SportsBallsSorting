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
            Time.timeScale = 1.0f;
            Hide();
        });

        quitButton.onClick.AddListener(() =>
        {
            GameManager.Instance.LoadMenuScene();
            Time.timeScale = 1.0f;
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
        replayButton.onClick.RemoveAllListeners();

        quitButton.onClick.RemoveAllListeners();
    }

    private void Hide()
    {
        gameObject.SetActive(false);

    }
}

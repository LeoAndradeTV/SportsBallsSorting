using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private Button resumeGame;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button closeButton;

    // Start is called before the first frame update
    void OnEnable()
    {
        resumeGame.onClick.AddListener(() =>
        {
            GameManager.Instance.ResumeGame();
        });

        quitButton.onClick.AddListener(() =>
        {
            GameManager.Instance.LoadMenuScene();
        });

        closeButton.onClick.AddListener(() =>
        {
            GameManager.Instance.ResumeGame();
        });
    }

    // Update is called once per frame
    void OnDisable()
    {
        resumeGame.onClick.RemoveListener(() =>
        {
            GameManager.Instance.ResumeGame();
        });

        quitButton.onClick.RemoveListener(() =>
        {
            GameManager.Instance.LoadMenuScene();
        });

        closeButton.onClick.RemoveListener(() =>
        {
            GameManager.Instance.ResumeGame();
        });
    }
}

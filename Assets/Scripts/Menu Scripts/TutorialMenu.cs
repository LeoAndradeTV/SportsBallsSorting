using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMenu : MonoBehaviour
{
    [SerializeField] private GameObject tutorialMenu;
    [SerializeField] private Button okButton;

    // Start is called before the first frame update
    void OnEnable()
    {
        okButton.onClick.AddListener(() =>
        {
            GameManager.Instance.CloseTutorialStartGame();
            tutorialMenu.SetActive(false);
        });
    }

    // Update is called once per frame
    void OnDisable()
    {
        okButton.onClick.RemoveListener(() =>
        {
            GameManager.Instance.CloseTutorialStartGame();
            tutorialMenu.SetActive(false);
        });
    }
}

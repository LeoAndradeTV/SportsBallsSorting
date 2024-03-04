using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject tutotialMenu;
    // Start is called before the first frame update
    void OnEnable()
    {
        GameManager.Instance.OnGameOver += ShowGameOverMenu;
        ToggleTutorialMenu(!GameManager.Instance.UserHasPlayedGame);
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameOver -= ShowGameOverMenu;
    }

    private void ShowGameOverMenu()
    {
        gameOverMenu.SetActive(true);
    }

    public void ToggleTutorialMenu(bool toggle)
    {
        // FOR BUILD, UNCOMMENT BELOW
        tutotialMenu.SetActive(toggle);

        // FOR TESTING, UNCOMMENT BELOW
        //tutotialMenu.SetActive(true);

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverMenu;
    // Start is called before the first frame update
    void OnEnable()
    {
        GameManager.Instance.OnGameOver += ShowGameOverMenu;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameOver -= ShowGameOverMenu;
    }

    private void ShowGameOverMenu()
    {
        gameOverMenu.SetActive(true);
    }

}

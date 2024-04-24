using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuCanvasManager : MonoBehaviour
{
    [SerializeField] private Button quitButton;

    private void OnEnable()
    {
        quitButton.onClick.AddListener(GameManager.Instance.CloseGame);
    }

    private void OnDisable()
    {
        quitButton.onClick.RemoveAllListeners();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Button backButton;

    // Start is called before the first frame update
    void OnEnable()
    {
        backButton.onClick.AddListener(() => GameManager.Instance.SaveMySettings());
    }

    private void OnDisable()
    {
        backButton.onClick.RemoveListener(() => GameManager.Instance.SaveMySettings());
    }
}

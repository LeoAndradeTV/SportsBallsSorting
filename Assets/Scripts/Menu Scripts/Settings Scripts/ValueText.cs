using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ValueText : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private void Start()
    {
        GetComponent<TMP_Text>().text = slider.value.ToString();

        slider.onValueChanged.AddListener(SetValue);
    }

    private void SetValue(float value)
    {
        GetComponent<TMP_Text>().text = value.ToString();
    }
}

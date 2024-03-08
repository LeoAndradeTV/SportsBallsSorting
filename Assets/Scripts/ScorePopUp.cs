using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ScorePopUp : MonoBehaviour
{
    private Vector3 amountToMoveUp = Vector3.up * 30f;

    public void UpdateText(int score)
    {
        TMP_Text scoreText = GetComponent<TMP_Text>();
        scoreText.text = "+" + score;
    }

    public IEnumerator Disappear()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position + amountToMoveUp;
        float maxTime = 2f;
        float currentTime = 0f;
        Color textColor = GetComponent<TMP_Text>().color;
        Color transparent = new Color(textColor.r, textColor.g, textColor.b, 0);
        while (currentTime < maxTime)
        {
            transform.position = Vector3.Lerp(startPos, endPos, currentTime/maxTime);
            GetComponent<TMP_Text>().color = Color.Lerp(textColor, transparent, currentTime/maxTime);
            currentTime += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
}

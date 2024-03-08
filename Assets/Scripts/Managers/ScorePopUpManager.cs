using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScorePopUpManager : MonoBehaviour
{
    [SerializeField] private RectTransform popUpHolder;
    [SerializeField] private Transform scorePopUpPrefab;

    // Start is called before the first frame update
    void Start()
    {
        BallCombineManager.Instance.OnBallCombined += PopUpScore;
    }

    private void PopUpScore(object sender, BallCombineManager.BallToCombineEventArgs e)
    {
        Transform scorePopUp = Instantiate(scorePopUpPrefab, transform);
        if (scorePopUp == null ) { return; }
        ScorePopUp popUp = scorePopUp.GetComponent<ScorePopUp>();
        popUp.UpdateText(e.ballToCombine.GetBallPoints());
        popUp.SetPosition(e.popUpLocation);
        StartCoroutine(popUp.Disappear());
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCard : MonoBehaviour
{
    [SerializeField] private TMP_Text rank;
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text playerScore;

    // Start is called before the first frame update
    void OnEnable()
    {
        
    }

    public void InitializeInformation(string rank, string name, string score)
    { 
        this.rank.text = rank;
        playerName.text = name;
        playerScore.text = score;
    }

    public void SetMyUsernameColor()
    {
        rank.color = Color.red;
        playerName.color = Color.red;
        playerScore.color = Color.red;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class LobbyScoreBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI vrScoreText, mobScoreText, scoreResult;

    [SerializeField]
    private Color vrColour, mobColour, defaultColour;


    void Start()
    {
        int vrScore = GameScoreManager.Instance.GetVRScore();
        int mobScore = GameScoreManager.Instance.GetMobileScore();
   
        vrScoreText.text = vrScore.ToString();
        mobScoreText.text = mobScore.ToString();

        if(vrScore > mobScore)
        {
            scoreResult.text = "Ringmaster Wins!!";
            scoreResult.color = vrColour;
        }
        else if (vrScore < mobScore)
        {
            scoreResult.text = "Clowns Win!!";
            scoreResult.color = mobColour;
        }
        else
        {
            scoreResult.text = "Draw!!";
            scoreResult.color = defaultColour;  
        }
        
    }

}

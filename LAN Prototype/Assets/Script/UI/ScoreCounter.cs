using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    
    public int vRScore;
    public int mobileScore;
    public List<TextMeshProUGUI> vRScoreText;
    public List<TextMeshProUGUI> mobileScoreText;

    

    private void Awake()
    {
        //subscribes UpdateScore to the OnScoreChanged event in the score manager
        if (GameScoreManager.Instance != null) GameScoreManager.Instance.OnScoreChanged += UpdateScore;
    }

    private void Start()
    {
        vRScore = 0;
        mobileScore = 0;
        for (int i = 0; i < vRScoreText.Count; i++)
        {
            vRScoreText[i].text = vRScore.ToString();
        }
        for (int i = 0; i < mobileScoreText.Count; i++)
        {
            mobileScoreText[i].text = mobileScore.ToString();
        }
    }

    
    //Function subscribed to event on ScoreManager, whenever the score is altered it will update the scores
    void UpdateScore(object sender, System.EventArgs e)
    {
        vRScore = GameScoreManager.Instance.GetVRScore();
        mobileScore = GameScoreManager.Instance.GetMobileScore();

        string vrString = vRScore.ToString();
        string mobileString = mobileScore.ToString();

        for (int i = 0; i < vRScoreText.Count; i++)
        {
            vRScoreText[i].text = vrString;
        }
        for (int i = 0; i < mobileScoreText.Count; i++)
        {
            mobileScoreText[i].text = mobileString;
        }
    }


}

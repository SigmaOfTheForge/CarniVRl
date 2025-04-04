using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using Unity.Netcode;

public class ScoreCounter : NetworkBehaviour
{
    
    public int vRScore;
    public int mobileScore;
    //is a list so multiple TextMeshProUGUI can be set to the same variables
    //this is so both the VR and mobile players can see the scores while 
    //looking at different directions
    public List<TextMeshProUGUI> vRScoreText;
    public List<TextMeshProUGUI> mobileScoreText;

    

    private void Awake()
    {
        //subscribes UpdateScore to the OnScoreChanged event in the score manager
        GameScoreManager.Instance.OnScoreChanged += UpdateScore;
    }

    private void Start()
    {
        vRScore = 0;
        GameScoreManager.Instance.SetVRScore(vRScore);
        mobileScore = 0;
        GameScoreManager.Instance.SetMobileScore(mobileScore);

        for (int i = 0; i < vRScoreText.Count; i++)
        {
            vRScoreText[i].text = vRScore.ToString();
        }
        for (int i = 0; i < mobileScoreText.Count; i++)
        {
            mobileScoreText[i].text = mobileScore.ToString();
        }
    }

    void UpdateScore(object sender, System.EventArgs e)
    {
       
        CallScoreUpdateServerRpc();
    }

    [ServerRpc]
    void CallScoreUpdateServerRpc()
    {
        Debug.Log("Score Update Server RPC Called");
        UpdateScoreClientRpc();
    }

    //Function subscribed to event on ScoreManager, whenever the score is altered it will update the scores
    [ClientRpc]
    void UpdateScoreClientRpc()
    {
        Debug.Log("Score Update Client RPC Called on client");
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

        Debug.Log("Score Updated on this client");
    }
}

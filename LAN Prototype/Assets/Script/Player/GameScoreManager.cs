using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;


public class GameScoreManager : NetworkBehaviour
{
    public static GameScoreManager Instance;

    private NetworkVariable<int> vrPlayerScore = new NetworkVariable<int>(0);
    private NetworkVariable<int> mobilePlayerScore = new  NetworkVariable<int>(0);

    //Event to be called whenever the score is modified, can be listened to for functionality
    public event EventHandler OnScoreChanged;

    private void Awake()
    {
        Instance = this;   


    }

    public int GetVRScore()
    {
        
        return vrPlayerScore.Value;
    }

    public void SetVRScore(int score)
    {
        EventArgs e = new EventArgs();
        OnScoreChanged.Invoke(this, e);
        vrPlayerScore.Value = score;
    }

    public void AddVRScore(int score)
    {
        Debug.Log("Score Manager/ VR Score added:  " +  score);
        EventArgs e = new EventArgs();
        OnScoreChanged.Invoke(this, e);
        vrPlayerScore.Value += score;
    }

    public int GetMobileScore()
    {
      
        return mobilePlayerScore.Value;
    }

    public void SetMobileScore(int score)
    {
        EventArgs e = new EventArgs();
        OnScoreChanged.Invoke(this, e);
        mobilePlayerScore.Value = score;
    }

    public void AddMobileScore(int score)
    {
        Debug.Log("Score Manager/ M Score added:  " + score);
        EventArgs e = new EventArgs();
        OnScoreChanged.Invoke(this, e);
        mobilePlayerScore.Value += score;
    }

}

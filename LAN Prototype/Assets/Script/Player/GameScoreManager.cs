using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class GameScoreManager : NetworkBehaviour
{
    public static GameScoreManager Instance;

    private NetworkVariable<int> vrPlayerScore = new NetworkVariable<int>(0);
    private NetworkVariable<int> mobilePlayerScore = new  NetworkVariable<int>(0);

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
        vrPlayerScore.Value = score;
    }

    public void AddVRScore(int score)
    {
        vrPlayerScore.Value += score;
    }

    public int GetMobileScore()
    {
        return mobilePlayerScore.Value;
    }

    public void SetMobileScore(int score)
    {
        mobilePlayerScore.Value = score;
    }

    public void AddMobileScore(int score)
    {
        mobilePlayerScore.Value += score;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDetails
{
    private string levelName;
    private int playerType;

    public void SetLevelName(string level)
    {
        levelName = level;
    }

    public void SetPlayerType(int type)
    {
        playerType = type;
    }

    public string GetLevelName()
    {
        return levelName;
    }

    public int GetPlayerType()
    {
        return playerType;
    }

}



public class LevelName : MonoBehaviour
{
    [SerializeField]
    private string levelName;

    [SerializeField]
    private int playerType;

    private LevelDetails levelDetails = new LevelDetails();

    private void Awake()
    {
        levelDetails.SetLevelName(levelName);
        levelDetails.SetPlayerType(playerType);
    }


    public LevelDetails GetLevelDetails()
    {
        return levelDetails;
    }
}

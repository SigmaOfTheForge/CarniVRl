using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using Unity.Services.Lobbies;

public class GameManager : NetworkBehaviour
{

    
    private int levelType = 0;

    //Network variables are synchronised across clients, all network variables must be initialised
    NetworkVariable<int> levelVariable = new NetworkVariable<int>();
    
    //gameManager should be persistent across scenes
    private void Awake()
    {
        levelVariable.Value = levelType;
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        //temporary inputs for starting levels
        if (Input.GetKeyDown(KeyCode.L))
        {
            
            ChangeSceneServerRpc("SceneTransitionTest", 1);
        }
        if(Input.GetKeyDown(KeyCode.K))
        {
            
            ChangeSceneServerRpc("Lobby", 0);
        }

    }



    //public scenechange so that anything can call the function from outside the GameManager
    public void ChangeScene(string sceneName, int type)
    {
        ChangeSceneServerRpc(sceneName, type);
    }

    //returns the current levelType for player spawning
    public int GetLevelType()
    {
        return levelVariable.Value;
    }


    //Loads levels across networks and calls all players to load into the scene
    [ServerRpc]
    void ChangeSceneServerRpc(string sceneName, int type)
    {
        levelVariable.Value = type;
        Debug.Log("Loading level, Level to load is: " + levelType);
        var status = NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        if (status != SceneEventProgressStatus.Started)
        {
            Debug.LogWarning("Failed to load " + sceneName + ", with a " + nameof(SceneEventProgressStatus) + ": " + status);

        }
       
    }





}

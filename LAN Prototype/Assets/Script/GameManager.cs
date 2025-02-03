using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{

    //[SerializeField]
    //private string sceneName;

    [SerializeField]
    private int levelType;

    
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            ChangeSceneServerRpc("SceneTransitionTest", 1);
        }
        if(Input.GetKeyDown(KeyCode.K))
        {
            ChangeSceneServerRpc("Lobby", 0);
        }
    }




    public void ChangeScene(string sceneName, int type)
    {
        ChangeSceneServerRpc(sceneName, type);
    }


    public int GetLevelType()
    {
        return levelType;
    }

   


    [ServerRpc]
    void ChangeSceneServerRpc(string sceneName, int type)
    {
        var status = NetworkManager.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        if (status != SceneEventProgressStatus.Started)
        {
            Debug.LogWarning("Failed to load " + sceneName + ", with a " + nameof(SceneEventProgressStatus) + ": " + status);

        }
        levelType = type;
    }



}

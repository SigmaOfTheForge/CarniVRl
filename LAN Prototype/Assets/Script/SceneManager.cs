using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class SceneManager : NetworkBehaviour
{

    [SerializeField]
    private string sceneName;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            ChangeSceneServerRpc();
        }
    }


    [ServerRpc]
    void ChangeSceneServerRpc()
    {
        var status = NetworkManager.SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        if(status != SceneEventProgressStatus.Started)
        {
            Debug.LogWarning("Failed to load " +  sceneName + ", with a " + nameof(SceneEventProgressStatus)+ ": "+ status);
        }
    }
}

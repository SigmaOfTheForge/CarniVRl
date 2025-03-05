using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using Unity.Services.Authentication;
using System;
using System.Threading.Tasks;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;

public class clientDisconnectManager : MonoBehaviour
{
    



    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;
        Debug.Log("Spawned on client: " + NetworkManager.Singleton.LocalClientId);

        AuthenticationService.Instance.SignedOut += () =>
        {
            Debug.Log("Signed out: " + AuthenticationService.Instance.PlayerId);
        };
    }




    private async void OnClientDisconnectCallback(ulong clientID)
    {
        Debug.Log("Callback called");

        //if (Application.platform == RuntimePlatform.WindowsPlayer)
        //{
        //    await LobbyService.Instance.DeleteLobbyAsync(LobbyService.Instance.GetJoinedLobbiesAsync().Result[0]);
        //}
        //else
        //{
            await LobbyService.Instance.RemovePlayerAsync(LobbyService.Instance.GetJoinedLobbiesAsync().Result[0], AuthenticationService.Instance.PlayerId);
        //}

        


        AuthenticationService.Instance.SignOut(true);

      
        Debug.Log("Client: " + clientID + " is disconnecting");
        GameObject netMan =NetworkManager.Singleton.gameObject;
        await Task.Delay(1000);
        
        Destroy(netMan);
        Debug.Log("Scene should be loaded here");
        SceneManager.LoadScene("SampleScene");

    }
}

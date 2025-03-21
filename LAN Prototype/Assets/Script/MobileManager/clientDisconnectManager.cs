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
        //if (Application.platform == RuntimePlatform.WindowsEditor) return;


        Debug.Log("Callback called");
        Debug.Log("ID is" + AuthenticationService.Instance.PlayerId + " - " + clientID);

        if(Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            GameObject.FindGameObjectWithTag("LobbyManager").GetComponent<TestLobby>().CloseLobby(AuthenticationService.Instance.PlayerId);
        }
        else
        {
            GameObject.FindGameObjectWithTag("LobbyManager").GetComponent<TestLobby>().DisconnectPlayer(AuthenticationService.Instance.PlayerId);
        }
       





        AuthenticationService.Instance.SignOut(true);

      
        //Debug.Log("Client: " + clientID + " is disconnecting");
        GameObject netMan =NetworkManager.Singleton.gameObject;
        await Task.Delay(1000);
        
        Destroy(netMan);
        //Debug.Log("Scene should be loaded here");
        SceneManager.LoadScene("SampleScene");

    }
}

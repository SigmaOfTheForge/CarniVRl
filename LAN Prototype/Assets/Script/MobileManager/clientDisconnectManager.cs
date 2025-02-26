using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using Unity.Services.Authentication;

public class clientDisconnectManager : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;
    }


    private void OnClientDisconnectCallback(ulong clientID)
    {

        AuthenticationService.Instance.SignOut();
        Debug.Log("Client: " + clientID + " is disconnecting");
        NetworkManager netMan = GameObject.FindObjectOfType<NetworkManager>();

        // Destroy(netMan);
        SceneManager.LoadScene("SampleScene");

    }
}

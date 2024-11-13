using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetcodeManagerUI : MonoBehaviour
{
    [SerializeField]
    private Button
        serverBtn,
        hostBtn,
        clientBtn;

    private void Awake()
    {
        serverBtn.onClick.AddListener(() =>
        {
            
            NetworkManager.Singleton.StartServer();
        });
        hostBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });
        clientBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
    }
}

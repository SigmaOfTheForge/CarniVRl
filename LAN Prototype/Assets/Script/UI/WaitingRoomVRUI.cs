using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingRoomVRUI : MonoBehaviour
{
    [SerializeField]
    private Button startButton, quitButton;

    void Start()
    {
        startButton.onClick.AddListener(() => StartGame());
        quitButton.onClick.AddListener(() => CloseLobby());
    }

    void StartGame()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().ChangeScene("Lobby", 0);
    }

    void CloseLobby()
    {
        GameObject.FindGameObjectWithTag("LobbyManager").GetComponent<TestLobby>().CloseLobby();
    }
}

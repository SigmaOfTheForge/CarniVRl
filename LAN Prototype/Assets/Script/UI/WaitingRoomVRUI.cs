using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingRoomVRUI : MonoBehaviour
{
    [SerializeField]
    private Button startButton, quitButton;

    // Start is called before the first frame update
    void Start()
    {


        startButton.onClick.AddListener(() => StartGame());
        quitButton.onClick.AddListener(() => CloseLobby());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartGame()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().ChangeScene("Lobby", 0);
    }

    void CloseLobby()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().CloseLobby();
    }
}

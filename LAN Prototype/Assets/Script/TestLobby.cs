using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using Unity.Netcode;
using System.Net;
using Unity.Netcode.Transports.UTP;
using UnityEngine.SceneManagement;
using static UnityEngine.AudioSettings;
//using UnityEditor.Experimental.GraphView;

public class TestLobby : MonoBehaviour
{
    [SerializeField]
    private GameObject mainCamera;

    //[SerializeField]
    private TextMeshProUGUI lobbyCodeText, playerNameText;

    //[SerializeField]
    private Button joinButton, createButton, listButton, qJoinButton;

    private string playerNameString, lobbyIPName;
    private Lobby hostLobby, joinedLobby;
    private float heatBeatTimer;

    private bool isHosting = false;

    // Start is called before the first frame update
    private async void Start()
    {
        await UnityServices.InitializeAsync();

        string name =  System.Net.Dns.GetHostName();
        IPAddress[] addr = System.Net.Dns.GetHostAddresses(name);

        foreach (IPAddress addrAddr in addr)
        {
            Debug.Log(addrAddr.ToString());
            if(addrAddr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                lobbyIPName = addrAddr.ToString();
                Debug.Log("Lobby name is: " + lobbyIPName);
            }
        }

        

        
       



        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        FindButtons();

    }



    private void Update()
    {
        HandleHeartBeat();
    }
    //function that does a simple function to keep lobby active
    private async void HandleHeartBeat()
    {
        if(hostLobby != null)
        {
            heatBeatTimer -= Time.deltaTime;
            if(heatBeatTimer < 0)
            {
                float heartBeatTimerMax = 15f;
                heatBeatTimer = heartBeatTimerMax;         
                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }
    }

    private async void CreateLobby()
    {
        //prevents double-clicks of lobby button and prevents 2 lobbies from the same computer appearing (i hope)
        if (isHosting) return;

        playerNameString = "Sigma[" + Random.Range(1,99) + "]" ;
        try
        {


            string lobbyName = lobbyIPName;
            int maxPlayers = 4;

            CreateLobbyOptions lobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = false,
                Player = GetPlayer(),
                Data = new Dictionary<string, DataObject>
                {
                    {"GameMode", new DataObject(DataObject.VisibilityOptions.Public, "Bowling") }
                }
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, lobbyOptions);
            hostLobby = lobby;

            ushort port = 7777;
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(lobbyIPName, port);


            PrintPlayers(hostLobby);



            Debug.Log("Created Lobby: " + lobby.Name + ", " + lobby.MaxPlayers + ", Password is: " +  lobby.LobbyCode);

            NetworkManager.Singleton.StartHost();

            isHosting = true;

            Destroy(GameObject.FindGameObjectWithTag("UI_Start"));
            Destroy(GameObject.FindGameObjectWithTag("MainCamera"));
            Destroy(GameObject.FindGameObjectWithTag("VR_Player_Start"));

            //GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelManager>().ChangeScene("Lobby", 0);

        }
        catch (LobbyServiceException e)
        {
            Debug.LogError(e);
        }

    }

    private async void ListLobbies()
    {
        try
        {
            //Lobby Filter options
            QueryLobbiesOptions qLobbyOptions = new QueryLobbiesOptions
            {
                Count = 25,
                Filters = new List<QueryFilter>
                {
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
                },
                Order = new List<QueryOrder>
                {
                    new QueryOrder(false, QueryOrder.FieldOptions.Created)
                }
            };
        
        QueryResponse qR = await Lobbies.Instance.QueryLobbiesAsync();
        Debug.Log("Lobbies Found: " + qR.Results.Count);
        foreach(Lobby lobby in qR.Results)
        {
            Debug.Log("   " + lobby.Name + " " + lobby.MaxPlayers + "\n\tActive Players: " + lobby.Players.Count );
        }
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError(e);
        }
    }
    //join lobby by code
    private async void JoinLobby(string code)
    {
        code = code.Substring(0, 6);
        //SceneManager.LoadScene("Lobby");
        try
        {
            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions
            {
                Player = GetPlayer()
            };

            joinedLobby = await Lobbies.Instance.JoinLobbyByCodeAsync(code, joinLobbyByCodeOptions);
            Debug.Log("Joined Lobby: " + hostLobby.Name + "With Code: " + code);

            PrintPlayers(joinedLobby);
            ushort port = 7777;
            lobbyIPName = joinedLobby.Name;
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(lobbyIPName, port);

            Debug.Log(NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address);

            NetworkManager.Singleton.StartClient();

            Destroy(GameObject.FindGameObjectWithTag("UI_Start"));
            Destroy(GameObject.FindGameObjectWithTag("VR_Player_Start"));

        }
        catch (LobbyServiceException e)
        {
            Debug.LogError(e);
        }
        //await Lobbies.Instance.JoinLobbyByIdAsync();
    }

    private Player GetPlayer()
    {
        return new Player
        {
            Data = new Dictionary<string, PlayerDataObject>
                    { {"PlayerName" , new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member,  playerNameString) } }

        };
    }

    //quickjoin an open lobby
    private async void QuickJoinLobby()
    {
        //SceneManager.LoadScene("Lobby");

        try
        {
            joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();
            Debug.Log("Joined Lobby: " + joinedLobby.Name);

            ushort port = 7777;
            lobbyIPName = joinedLobby.Name;

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(lobbyIPName, port);
            Debug.Log(NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.ToString());

            NetworkManager.Singleton.StartClient();

            Destroy(GameObject.FindGameObjectWithTag("UI_Start"));
            Debug.Log("About to destroy camera: " + mainCamera);
            Destroy(mainCamera);
            Debug.Log("Destroyed camera: " + mainCamera);

        }
        catch (LobbyServiceException e)
        {
            Debug.LogError(e);
        }
    }

    private void PrintPlayers(Lobby lobby)
    {
        if (lobby == null) return;

        
        Debug.Log("Players in Lobby " + lobby.Name);
        foreach(Player player in lobby.Players)
        {
            Debug.Log(player.Id + ": " + player.Data["PlayerName"].Value);
        }
    }

    private void FindButtons()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            joinButton = GameObject.FindGameObjectWithTag("Button_Join").GetComponent<Button>();
            qJoinButton = GameObject.FindGameObjectWithTag("Button_QJ").GetComponent<Button>();
            listButton = GameObject.FindGameObjectWithTag("Button_ListLob").GetComponent<Button>();
            lobbyCodeText = GameObject.FindGameObjectWithTag("UIInput_Code").GetComponent<TextMeshProUGUI>();
            playerNameText = GameObject.FindGameObjectWithTag("UIInput_Name").GetComponent<TextMeshProUGUI>();

            joinButton.onClick.AddListener(() => JoinLobby(lobbyCodeText.text));
            listButton.onClick.AddListener(() => ListLobbies());
            qJoinButton.onClick.AddListener(() => QuickJoinLobby());
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
           createButton = GameObject.FindGameObjectWithTag("Button_Create").GetComponent<Button>();
           listButton = GameObject.FindGameObjectWithTag("Button_ListLob").GetComponent<Button>();
           playerNameText = GameObject.FindGameObjectWithTag("UIInput_Name").GetComponent<TextMeshProUGUI>();

            listButton.onClick.AddListener(() => ListLobbies());
            createButton.onClick.AddListener(() => CreateLobby());

        }
    }


    public void SearchForButtons()
    {
        FindButtons();
    }


  
}

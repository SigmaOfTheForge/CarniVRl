using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using System.Globalization;
using UnityEngine.UI;
using Unity.VisualScripting;
using Unity.Services.Core;
using Unity.Services.Authentication;

public class MobilePlayerManager : NetworkBehaviour
{
    [SerializeField]
    private NetworkObject[] mobPlayerType;
    [SerializeField]
    private NetworkObject currentMobPlayer;
    [SerializeField]
    private GameObject menuUI, clientDisconnectManager;

    private int playerNumber;

    private GameObject currentMenu;

    private GameObject[] spawnPoints;

    private ulong clientID;

    private Button disconnectButton;

    
    //Takes place before OnNetworkSpawn
    private void Awake()
    {
        
        //Delegate attached to NSM, when *all clients* have loaded it triggers OnLoadScene
        NetworkManager.Singleton.SceneManager.OnLoadComplete += OnLoadScene;
        
       

    }

    //Takes place after manager spawns on the network
    private void Start()
    {
        if (!IsOwner) return;

        

        clientID = OwnerClientId;

        //if there is no player in the scene, spawn a player on the network 
        if (currentMobPlayer == null)
        {
            
            int levelType = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().GetLevelType();

            // Gets all spawnPoints
            GameObject[] tempArray = GameObject.FindGameObjectsWithTag("Mob_Spawn");
            System.Array.Resize(ref spawnPoints, tempArray.Length);
            spawnPoints = tempArray;

            //instantiates and spawns the player
            SpawnOnNetworkMobileServerRpc(levelType, clientID);



        }
        if (!currentMenu)
        {
            currentMenu = Instantiate(menuUI, this.transform);
            disconnectButton = currentMenu.transform.Find("Panel").Find("disconnectButton").GetComponent<Button>();
            disconnectButton.onClick.AddListener(() => DisconButtonPressed());
        }

        Instantiate(clientDisconnectManager);
        

    }


    //Spawns the level-specific player when all clients have finished loading the level
    private void OnLoadScene(ulong clientID, string sceneName, LoadSceneMode loadSceneMode)
    {
        if (!IsOwner) return;


        



        int levelType = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().GetLevelType();

        //despawns player if one is still referenced
        if (currentMobPlayer != null)
        {
            DespawnOnNetworkMobileServerRpc();
        }

        //finds all spawnpoints in the level
        GameObject[] tempArray  = GameObject.FindGameObjectsWithTag("Mob_Spawn");
        System.Array.Resize(ref spawnPoints, tempArray.Length);
        spawnPoints = tempArray;

        //instantiates and spawns the player
        SpawnOnNetworkMobileServerRpc(levelType, clientID);

      

    }
    //when the player is spawned by the separator their player number is set
    public void SetPlayerNumber(int playerN)
    {
        playerNumber = playerN;
    }

    public void SetClient(ulong client)
    {
        clientID = client;
        Debug.Log(client);

    }

    private void DisconButtonPressed()
    {
        DespawnOnNetworkMobileServerRpc();
        DisconnectPlayerServerRpc(OwnerClientId);
    }




    [ServerRpc]
    void DisconnectPlayerServerRpc(ulong clientID)
    {

        NetworkManager.Singleton.DisconnectClient(clientID);
        this.NetworkObject.Despawn();

    }



    //Instantiates a specified player type and spawns them on the network owned by this GameObject
    [ServerRpc]
    void SpawnOnNetworkMobileServerRpc(int playerType, ulong ownerID)
    {
       
        Debug.Log("Player type to spawn is: " + playerType);
        

        NetworkObject player = Instantiate(mobPlayerType[playerType]);

        Debug.Log("player is: " + player );

        player.SpawnWithOwnership(ownerID, true);
       

        currentMobPlayer = player;

        SetPlayerClientRpc(player);

         
    }

    //Recieves a reference of the spawned player from the server and sets them as the current player
    [ClientRpc]
    void SetPlayerClientRpc(NetworkObjectReference player)
    {
        if(!IsOwner) return;
        bool gotPlayer = player.TryGet(out currentMobPlayer);

        //sets position of the player once it recieves the reference
        if(spawnPoints.Length > 0)
        {
            currentMobPlayer.transform.position = spawnPoints[playerNumber].transform.position;
            currentMobPlayer.transform.rotation = spawnPoints[playerNumber].transform.rotation;
        }

    }

    //Despawns the player on the network
    [ServerRpc]
    void DespawnOnNetworkMobileServerRpc()
    {
        currentMobPlayer.Despawn();
    }

}

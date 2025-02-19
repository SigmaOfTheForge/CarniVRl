using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;


public class VRPlayerManager : NetworkBehaviour
{

  


    [SerializeField]
    private NetworkObject[] vrPlayerType;

    [SerializeField]
    private GameObject[] vrPlayerHead;

    [SerializeField]
    private NetworkObject currentPlayer;

    private Transform spawnPoint;

    private ulong clientID;

    //public event NetworkSceneManager.OnLoadCompleteDelegateHandler sceneLoaded;

    private void Awake()
    {



        //Delegate to OnLoadComplete, called when all clients have finished loading a scene
        NetworkManager.Singleton.SceneManager.OnLoadComplete += OnLoadScene;

    }


    // Start is called before the first frame update
    void Start()
    {
         if (!IsOwner) return;



         //if there is no player in the scene, spawns a new one 
        if (currentPlayer == null)
        {
            //finds spawnpoints
            spawnPoint = GameObject.FindGameObjectWithTag("VR_Spawn").transform;
            //spawns player
            currentPlayer = Instantiate(vrPlayerType[0], gameObject.transform);
            //spawn the playe on the network
            SpawnOnNetworkServerRpc(clientID);
            //set player position
            currentPlayer.transform.position = spawnPoint.transform.position;
            currentPlayer.transform.rotation = spawnPoint.transform.rotation;

        }
    }



    //When the client has finished loading the scene
    private void OnLoadScene(ulong clientID, string sceneName, LoadSceneMode loadSceneMode)
    {
        if (!IsOwner) return;



        int levelType = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().GetLevelType();
        //if there is a player, destroy and despawn them
        if(currentPlayer != null)
        {
            DespawnOnNetworkServerRpc();
        }

        //finds spawnpoint and spawns the player
        spawnPoint = GameObject.FindGameObjectWithTag("VR_Spawn").transform;   
        currentPlayer = Instantiate(vrPlayerType[levelType], gameObject.transform);
        SpawnOnNetworkServerRpc( clientID);


        //moves player to the spawnpoint
        currentPlayer.transform.position = spawnPoint.transform.position;
        currentPlayer.transform.rotation = spawnPoint.transform.rotation;
    }

    public Transform GetCurrentPlayerTransform()
    {
        if (!currentPlayer) Debug.Log("No player assigned at time of call");

        return currentPlayer.transform;
    }

    public void SetClient(ulong client)
    {
        clientID = client;
        Debug.Log(client);
    }

    //spawns the correct player type in the level and on the network
    [ServerRpc]
    void SpawnOnNetworkServerRpc( ulong ownerID, ServerRpcParams serverRpcParams = default)
    {


       // if (!IsOwner) return;

        currentPlayer.SpawnWithOwnership(ownerID, true);
    }

    //Despawns and destroys the currentPlayer
    [ServerRpc]
    void DespawnOnNetworkServerRpc()
    {
        currentPlayer.Despawn();
    }

}

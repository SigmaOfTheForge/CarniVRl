using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class MobilePlayerManager : MonoBehaviour
{
    [SerializeField]
    private NetworkObject[] mobPlayerType;
    [SerializeField]
    private NetworkObject currentPlayer;

    private int playerNumber;

    private GameObject[] spawnPoints;

    private ulong clientID;

    private void Awake()
    {
        //clear any prior callback and then create a new one
        SceneManager.sceneLoaded -= OnLoadScene;
        SceneManager.sceneLoaded += OnLoadScene;

    }


    // Start is called before the first frame update
    void Start()
    {
        if (currentPlayer == null)
        {
            spawnPoints = GameObject.FindGameObjectsWithTag("Mob_Spawn");
            currentPlayer = Instantiate(mobPlayerType[0], gameObject.transform);
            SpawnOnNetworkServerRpc(currentPlayer, clientID);
            currentPlayer.transform.position = spawnPoints[playerNumber].transform.position;
            currentPlayer.transform.rotation = spawnPoints[playerNumber].transform.rotation;

        }
    }

    private void OnLoadScene(Scene scene, LoadSceneMode mode)
    {
        int levelType = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().GetLevelType();
        if (currentPlayer != null)
        {
            currentPlayer.Despawn();
        }
        spawnPoints = GameObject.FindGameObjectsWithTag("Mob_Spawn");
        currentPlayer = Instantiate(mobPlayerType[levelType], gameObject.transform);
        SpawnOnNetworkServerRpc(currentPlayer, clientID);
        currentPlayer.transform.position = spawnPoints[playerNumber].transform.position;
        currentPlayer.transform.rotation = spawnPoints[playerNumber].transform.rotation;
    }

    public void SetPlayerNumber(int playerN)
    {
        playerNumber = playerN;
    }

    public void SetClient(ulong client)
    {
        clientID = client;
    }

    [ServerRpc]
    void SpawnOnNetworkServerRpc(NetworkObject objToSpawn, ulong ownerID)
    {

        objToSpawn.SpawnWithOwnership(ownerID);
    }
}

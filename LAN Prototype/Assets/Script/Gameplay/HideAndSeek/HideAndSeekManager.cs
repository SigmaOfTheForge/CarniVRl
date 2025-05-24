using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public enum GameMode
{
    Start, Gameplay, End
}


public class HideAndSeekManager : NetworkBehaviour
{

    [SerializeField]
    private float respawnTimer, timePerPlayer, waitTime, gameDuration;

    private float gameTimer, scoreTimer;

    private GameMode gameMode;

    GameObject[] spawnPoints;

    public static HideAndSeekManager instance;

    //start a game
    private void Start()
    {
        instance = this;

        //get all current spawnpoints, can add as many spawnpoints as needed
        GameObject[] sPoints = GameObject.FindGameObjectsWithTag("Mob_Spawn");
        System.Array.Resize(ref spawnPoints, sPoints.Length);
        spawnPoints = sPoints;
        if (IsServer)
        {
            GameScoreManager.Instance.SetVRScore(0);
            GameScoreManager.Instance.SetMobileScore(0);
        }

        gameMode = GameMode.Start;
        scoreTimer = timePerPlayer * 3;
        gameTimer = waitTime + timePerPlayer;

    }

    private void Update()
    {
        if(!IsServer) return;

        gameTimer -= Time.deltaTime;

        switch (gameMode)
        {
            case GameMode.Start:
                if(gameTimer <= 0)
                {
                    gameMode = GameMode.Gameplay;
                    gameTimer = gameDuration;
                }
                break;
            case GameMode.Gameplay:
                scoreTimer -= Time.deltaTime;
                if(scoreTimer <= 0)
                {
                    scoreTimer = timePerPlayer;
                    GameScoreManager.Instance.AddMobileScore(1);
                }

                if(gameTimer <= 0)
                {
                    gameMode=GameMode.End;
                }
                break;
            case GameMode.End:
                if(gameTimer <= 0)
                {
                    GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().ChangeScene("Lobby", 0);
                }
                break;  

        }
    }


    //keep score

    //manage decoys?
    public void DecoyCaught()
    {
        GameScoreManager.Instance.AddMobileScore(1);
    }

    //Respawn a player when they get caught

    public void PlayerCaught(NetworkObjectReference player)
    {
        Debug.Log("Player: " +  player + ", has been caught");

        StartCoroutine(RespawnDelay(player));

        scoreTimer += timePerPlayer;

        GameScoreManager.Instance.AddVRScore(1);
    }

    //Disconnects the player from the grab and begins respawn on all clients
    private IEnumerator RespawnDelay(NetworkObjectReference player)
    {
        yield return new WaitForSeconds(respawnTimer);

        NetworkObject playerObject;
        player.TryGet(out playerObject);
        
        playerObject.GetComponent<XRGrabInteractable>().enabled = false;

        RespawnPlayerClientRpc(player);

        playerObject.GetComponent<XRGrabInteractable>().enabled = true;

    }


    //respawns on all clients
    [ClientRpc]
    private void RespawnPlayerClientRpc(NetworkObjectReference player)
    {
        Debug.Log("Respawning Player");
        
        NetworkObject playerObject;
        player.TryGet(out playerObject);
        playerObject.SendMessage("PlayerReset");

        int randomNumber = Random.Range(0, spawnPoints.Length);

        if (playerObject)
        {
            playerObject.transform.rotation = spawnPoints[randomNumber].transform.rotation;
            playerObject.transform.position = spawnPoints[randomNumber].transform.position;
            playerObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            Debug.Log("Respawned Player at " + spawnPoints[randomNumber].transform.position);
        }
    }

}

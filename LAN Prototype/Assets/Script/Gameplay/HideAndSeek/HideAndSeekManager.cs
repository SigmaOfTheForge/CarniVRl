using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HideAndSeekManager : NetworkBehaviour
{

    [SerializeField]
    private float respawnTimer;


    GameObject[] spawnPoints;



    //start a game
    private void Start()
    {
        //get all current spawnpoints, can add as many spawnpoints as needed
        GameObject[] sPoints = GameObject.FindGameObjectsWithTag("Mob_Spawn");
        System.Array.Resize(ref spawnPoints, sPoints.Length);
        spawnPoints = sPoints;
    }
    //keep score

    //manage decoys?

    //Respawn a player when they get caught

    public void PlayerCaught(NetworkObjectReference player)
    {
        Debug.Log("Player: " +  player + ", has been caught");

        StartCoroutine(RespawnDelay(player));

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

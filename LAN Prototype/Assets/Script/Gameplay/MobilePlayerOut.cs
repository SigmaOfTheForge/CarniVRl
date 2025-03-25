using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering.Universal;

//goes on the barrier the mobile player hits after getting knocke outwadaw
public class MobilePlayerOut : NetworkBehaviour
{
    [SerializeField] private List<Transform> mobileSpawns;
    [SerializeField] private int playerRespawnTimer;

    private void OnTriggerEnter(Collider other)
    {

        if (!IsHost) return; //ensures only the host will call the RPC to not recieve multiple calls


        Debug.Log("Triggrt hit!");

        if (other.tag == "Mobile_Player") 
        {
          

            if (GameScoreManager.Instance != null) GameScoreManager.Instance.AddVRScore(1);

            RespawnPlayerClientRpc(other.GetComponent<NetworkObject>());
        }
        else
        {
            other.gameObject.SetActive(false);
        }
    }


    private void RespawnPlayer(Collider other)
    {
        Debug.Log("Attempting to respawnPlayer");

        other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        other.transform.position = mobileSpawns[Random.Range(0, mobileSpawns.Count -1)].transform.position;
        other.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        //other.SendMessage("ToggleMove");

    }

    //Uses an RPC so that changes are synchronised across clients
    [ClientRpc]
    private void RespawnPlayerClientRpc(NetworkObjectReference playerObjectRef )
    {

        NetworkObject playerObject;
        playerObjectRef.TryGet(out playerObject);

   
        //playerObject.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;

        //playerObject.SendMessage("ToggleMove");

       

        playerObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        playerObject.transform.position = mobileSpawns[Random.Range(0, mobileSpawns.Count -1)].position;
        playerObject.SendMessage("StartGrace");
        playerObject.transform.GetComponent <MeshRenderer>().enabled = true;

        //playerObject.SendMessage("ToggleMove");


    }

}

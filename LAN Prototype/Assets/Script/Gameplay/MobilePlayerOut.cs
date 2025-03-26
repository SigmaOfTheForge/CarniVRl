using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
//goes on the barrier the mobile player hits after getting knocke outwadaw
public class MobilePlayerOut : NetworkBehaviour
{
    [SerializeField] private List<Transform> mobileSpawns;
    [SerializeField] private int playerRespawnTimer;
    //When a M-Player is launched into the target at the back of the level
    private void OnTriggerEnter(Collider other)
    {
        if (!IsHost) return; //ensures only the host will call the RPC to not recieve multiple calls

        if (other.tag == "Mobile_Player") 
        {
            //Give the VR-Player a score
            GameScoreManager.Instance.AddVRScore(1);
            //Respawn the player
            RespawnPlayerClientRpc(other.GetComponent<NetworkObject>());
        }
        else //if it is a ball that was thrown
        {
            other.gameObject.SetActive(false);
        }
    }
    //Uses an RPC so that changes are synchronised across clients
    [ClientRpc]
    private void RespawnPlayerClientRpc(NetworkObjectReference playerObjectRef )
    {

        NetworkObject playerObject;
        playerObjectRef.TryGet(out playerObject);
        //Resets its velocity & position and gives it a brief bit of immunity from being hit again
        playerObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        playerObject.transform.position = mobileSpawns[Random.Range(0, mobileSpawns.Count -1)].position;
        playerObject.SendMessage("StartGrace");
        playerObject.transform.GetComponent<MeshRenderer>().enabled = true;
    }
}

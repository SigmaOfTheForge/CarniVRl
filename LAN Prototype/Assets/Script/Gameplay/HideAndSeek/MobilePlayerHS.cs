using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode.Components;

public class MobilePlayerHS : NetworkBehaviour
{
    //XR Origin and its interactions only take place server-side (theoretically)


    private bool isCaught;

    // Update is called once per frame
    void Update()
    {
        //Sends transform data to all clients
        if (isCaught)
        {
            SynchronisePositionClientRpc(gameObject.transform.position, gameObject.transform.rotation);
        }
    }

    //Called by XR Grabbable, starts synchronising position across clients and fires a function on GameManager to handle respawning and scores
    public void PlayerPickedUp()
    {
        if(!IsServer) return;
        isCaught = true;
       NetworkObject player =  GetComponent<NetworkObject>();
        GameObject.FindGameObjectWithTag("Finish").GetComponent<HideAndSeekManager>().PlayerCaught(player);
    }

    //disables the synchroniseation
    public void PlayerReset()
    {
        isCaught= false;
    }

    //synchronises this game object's transform to the server-side one
    [ClientRpc]
    private void SynchronisePositionClientRpc(Vector3 position, Quaternion rotation)
    {
        if(IsServer) return;

        gameObject.transform.position = position;
        gameObject.transform.rotation = rotation;  

    }





}

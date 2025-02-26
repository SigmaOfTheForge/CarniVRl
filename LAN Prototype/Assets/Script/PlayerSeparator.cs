using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerSeparator : NetworkBehaviour
{
    [SerializeField]
    private NetworkObject pcPlayer, mobilePlayer;


    // Start is called before the first frame update
    void Start()
    {


    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (!IsOwner) return;

        ulong clientID = NetworkManager.Singleton.LocalClientId;
        Debug.Log("[PS]LocalClient is: " + clientID);

        
         if (Application.platform == RuntimePlatform.Android)
        {

            SpawnOnNetworkServerRpc(0, clientID);
            //player.GetComponent<NetworkObject>().Spawn();


            //Destroy(this);
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            //Debug.Log(clientID);
            SpawnOnNetworkServerRpc(1, clientID);
            //player.GetComponent<NetworkObject>().Spawn();


            //Destroy(this);
        }

    }

    //Spawn the player and spawn them on the network
    [ServerRpc]
    void SpawnOnNetworkServerRpc(int playerType, ulong clientID)
    {
        NetworkObject player;

       

        Debug.Log("Client in PS-RPC is: " + clientID);

        switch (playerType)
        {
            //Spawn Mobile player
            case 0:
                player = Instantiate(mobilePlayer);
                //DDOLManager.instance.AddDDOLObject(player.gameObject);
                DontDestroyOnLoad(player);

                player.SpawnWithOwnership(clientID, false);
                break;

            //Spawn PC/VR player
            case 1:
                
                player = Instantiate(pcPlayer);
                //DDOLManager.instance.AddDDOLObject(player.gameObject);
                DontDestroyOnLoad(player);
                player.SpawnWithOwnership( clientID, false);
              
                break;

           


        }
        //destroy PlayerSeparator as is no longer needed and destroy on the network
        this.GetComponent<NetworkObject>().Despawn();
    }


}

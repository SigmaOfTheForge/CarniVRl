using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MobilePlayerHS : NetworkBehaviour
{
    //XR Origin and its interactions only take place server-side (theoretically)

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerPickedUp()
    {
        if(!IsServer) return;

       NetworkObject player =  GetComponent<NetworkObject>();
        GameObject.FindGameObjectWithTag("Finish").GetComponent<HideAndSeekManager>().PlayerCaught(player);
    }



}

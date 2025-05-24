using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DecoyBehaviour : MonoBehaviour
{

    public void Grabbed()
    {
        HideAndSeekManager.instance.DecoyCaught();
        StartCoroutine(DecoyDelay());
    }


    private IEnumerator DecoyDelay()
    {
        NetworkObject nO = GetComponent<NetworkObject>();

        yield return new WaitForSeconds(1);
        
        //Send message to Owner that allows them to spawn a new one

        nO.Despawn();
    }

}

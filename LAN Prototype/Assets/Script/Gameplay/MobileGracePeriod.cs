using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MobileGracePeriod : NetworkBehaviour
{
    private CapsuleCollider capsuleCollider;
    private MeshRenderer gracePeriodVisualIndicator;

    private void Start()
    {
        


        capsuleCollider = this.gameObject.GetComponent<CapsuleCollider>();
        gracePeriodVisualIndicator = this.gameObject.GetComponent<MeshRenderer>();
        gracePeriodVisualIndicator.enabled = false;
    }

    public void StartGrace()
    {
        
        Debug.Log("Enabled");
       ChangeLayerClientRpc();
        ToggleVisibilityActiveClientRpc();
        StartCoroutine(GraceCountdown());
    }

    private IEnumerator GraceCountdown()
    {
        Debug.Log("Coroutine Started");
        yield return new WaitForSeconds(2);
        Debug.Log("Time Elapsed, deactivating protections");
        ChangeLayerClientRpc();
        ToggleVisibilityDeactiveClientRpc();
        //gameObject.SendMessage("ToggleMove");
       
    }

    [ClientRpc]
    private void ToggleVisibilityDeactiveClientRpc()
    {
        gracePeriodVisualIndicator.enabled = false;
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;

    }

    [ClientRpc]
    private void ToggleVisibilityActiveClientRpc()
    {
        gracePeriodVisualIndicator.enabled = true;
        this.gameObject.GetComponent<MeshRenderer>().enabled = true;
    }

    [ClientRpc]
    private void ChangeLayerClientRpc()
    {
        if(gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            gameObject.layer = LayerMask.NameToLayer("Bowling-Ball");
            capsuleCollider.excludeLayers = LayerMask.GetMask("Bowling-Ball");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
            capsuleCollider.excludeLayers = LayerMask.GetMask("Nothing");
        }
    }

}

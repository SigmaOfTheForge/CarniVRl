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
        if (!IsOwner) return;


        capsuleCollider = this.GetComponent<CapsuleCollider>();
        gracePeriodVisualIndicator = this.GetComponent<MeshRenderer>();
        gracePeriodVisualIndicator.enabled = false;
    }

    private void OnEnable()
    {
        if(!IsOwner) return;
        Debug.Log("Enabled");
       ChangeLayerClientRpc();
        ToggleVisibilityClientRpc();
        StartCoroutine(GraceCountdown());
    }

    private IEnumerator GraceCountdown()
    {
        Debug.Log("Coroutine Started");
        yield return new WaitForSeconds(2);
        Debug.Log("Time Elapsed, deactivating protections");
        ChangeLayerClientRpc();
        ToggleVisibilityClientRpc();
        //gameObject.SendMessage("ToggleMove");
       
    }

    [ClientRpc]
    private void ToggleVisibilityClientRpc()
    {
        gracePeriodVisualIndicator.enabled = !gracePeriodVisualIndicator.enabled;
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

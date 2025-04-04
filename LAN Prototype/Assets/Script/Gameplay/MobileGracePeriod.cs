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
        //makes sure the grace period indicator is off by default
        gracePeriodVisualIndicator.enabled = false;
    }

    public void StartGrace()
    {
        //make the player briefly invincible to balls after respawning
        Debug.Log("Enabled");
        ChangeLayerClientRpc();
        ToggleVisibilityActiveClientRpc();
        StartCoroutine(GraceCountdown());
    }

    //the amount of time that the grace period is up for
    private IEnumerator GraceCountdown()
    {
        yield return new WaitForSeconds(2);  
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

    //makes sure that the player is immune to bowling balls 
    //during the grace period and is set back to not immune 
    //when the grace period ends
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

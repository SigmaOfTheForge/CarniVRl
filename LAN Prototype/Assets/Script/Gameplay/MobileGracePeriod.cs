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

        gameObject.layer = LayerMask.NameToLayer("Bowling-Ball");
        capsuleCollider.excludeLayers = LayerMask.GetMask("Bowling-Ball");
        gracePeriodVisualIndicator.enabled = true;
        StartCoroutine(GraceCountdown());
    }

    private IEnumerator GraceCountdown()
    {
        yield return new WaitForSeconds(2);
        gracePeriodVisualIndicator.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Default");
        capsuleCollider.excludeLayers = LayerMask.GetMask("Nothing");
    }
}

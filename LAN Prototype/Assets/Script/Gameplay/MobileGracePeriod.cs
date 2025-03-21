using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileGracePeriod : MonoBehaviour
{
    private CapsuleCollider capsuleCollider;
    private MeshRenderer gracePeriodVisualIndicator;

    private void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        gracePeriodVisualIndicator = GetComponent<MeshRenderer>();
        gracePeriodVisualIndicator.enabled = false;
    }

    private void OnEnable()
    {
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

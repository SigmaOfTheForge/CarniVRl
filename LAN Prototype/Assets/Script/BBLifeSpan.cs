using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BBLifeSpan : NetworkBehaviour
{
    [SerializeField] private float lifeTime;
    [SerializeField] private float setLifeTime;

    private void OnEnable()
    {
        if (IsServer) // Only the server controls lifespan
        {
            lifeTime = setLifeTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!IsServer) return;

        if (collision.gameObject.tag == "Ramp")
        {
            StartCoroutine(LifeCountdown());
        }
    }

    IEnumerator LifeCountdown()
    {
        while (lifeTime > 0)
        {
            yield return new WaitForSeconds(1f);
            lifeTime -= 1f;
        }

        DeactivateObjectClientRpc();
    }

    [ClientRpc]
    void DeactivateObjectClientRpc()
    {
        gameObject.SetActive(false);
    }
}
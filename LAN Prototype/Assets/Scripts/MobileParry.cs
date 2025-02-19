using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MobileParry : NetworkBehaviour
{
    [SerializeField] Transform vrPlayer;
    bool isParrying;

    private void Start()
    {
        vrPlayer = GameObject.FindGameObjectWithTag("VR_Player_Manager").GetComponent<VRPlayerManager>().GetCurrentPlayerTransform();
    }

    void Update()
    {
        if (Input.GetKeyDown("e")) //get mobile button
        {
            HandleBlockPressed();
            isParrying = true;
        }
        if (Input.GetKeyUp("e")) //get mobile button
        {
            HandleBlockReleased();
            isParrying = false;
        }
    }

    private void HandleBlockPressed()
    {
        ParryManager.SharedInstance.StartParryWindow();
    }

    private void HandleBlockReleased()
    {
        ParryManager.SharedInstance.ResetParryWindow();
    }

    private void OnAttackRecieved(bool canParry, bool canBlock, GameObject ball, Rigidbody brb, Transform cannon)
    {
        ParryManager.SharedInstance.HandleAttack(canParry, canBlock, ball, brb, cannon);
    }
    
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "CannonBall")
        {
            Rigidbody brb = other.gameObject.GetComponent<Rigidbody>();
            
            if (isParrying)
            {
                OnAttackRecieved(true, true, other.gameObject, brb, vrPlayer);
            }
            else
            {
                OnAttackRecieved(false, false, other.gameObject, brb, vrPlayer);
            }
        }
    }
}

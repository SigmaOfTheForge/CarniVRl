using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;


public class MobileHSMovement : Movement
{
    [SerializeField]
    private GameObject scoreUI, decoyGameObject;

    [SerializeField]
    private float turnSpeed;

    private PlayerInput playerInput;

    private bool isDecoyPressed, isDecoySpawned;

    private float horizontalLookInput;


    public override void Start()
    {
        base.Start();
        Instantiate(scoreUI);

        //TD: add player input for it spawning decoys
        playerInput = GetComponent<PlayerInput>();
    }



    public override void Update()
    {
        base.Update();
        Look();
        if(IsOwner && playerInput.actions["Shield"].IsPressed() && !isDecoyPressed)
        {
            isDecoyPressed = true;
            SpawnDecoy();
        }
        if(IsOwner && !playerInput.actions["Shield"].IsPressed())
        {
            isDecoyPressed = false;
        }
    }

    public void DecoyDestroyed()
    {
        isDecoySpawned = false;
    }


    private void SpawnDecoy()
    {
        if(!isDecoySpawned)
        {
            isDecoySpawned = true;
            SpawnDecoyObjectServerRpc(OwnerClientId);
        }
    }


    private void Look()
    {
        if (!IsOwner || !canMove.Value) return;

        if (plInput.actions["Look"].ReadValue<Vector2>() != Vector2.zero)
        {
            horizontalLookInput = plInput.actions["Look"].ReadValue<Vector2>().x * turnSpeed * Time.deltaTime;
        }
        else
        {
            horizontalLookInput = 0;
        }
        transform.Rotate(Vector3.up * horizontalLookInput);
    }

    [ServerRpc]
    private void SpawnDecoyObjectServerRpc(ulong clientID)
    {
        GameObject decoy = Instantiate(decoyGameObject, this.transform.position, this.transform.rotation);
        decoy.GetComponent<NetworkObject>().SpawnWithOwnership(clientID);
    }

}

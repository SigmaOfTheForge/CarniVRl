using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;
using System;

public class MobileParry : NetworkBehaviour
{
    private IEnumerator parryAttackWindow; //IEnumerator for parryAttackWindow so that it can be null checked
    private bool isParryEnabled = false; //checks if parry is enabled
    [SerializeField] private float parryWindow; //how long the player has to parry
    [SerializeField] private GameObject parryShieldObj;

    Rigidbody rb;
    float force = 20f;

    [SerializeField] private PlayerInput parryAction;
    
    [SerializeField] Transform vrPlayer;
    bool isParryButtonPressed; //checks if the button binded to the shield is pressed;

    [SerializeField] private PlayerInput parryControls;

    private void Start()
    {
        if (!IsOwner) return;

        rb = GetComponent<Rigidbody>();

        //Deactivate shield visibility and find VR Player in the scene
        CallToggleServerRpc();
        vrPlayer = GameObject.FindGameObjectWithTag("VR_Player_Start").transform;

        //gets the action mappings
        parryAction = gameObject.GetComponent<PlayerInput>();
      
    }

    public void Update()
    {
        if (!IsOwner) return;

        //Checks if the button assigned to "Sheild" is pressed down
        if (parryAction.actions["Shield"].IsPressed() && !isParryButtonPressed)
        {
            isParryButtonPressed = true;
            Debug.Log("Shield up");
            StartParryWindow();
        }
        //Checks if the button assigned to "Shield" is let go
        else if (!parryAction.actions["Shield"].IsPressed() && isParryButtonPressed)
        {
            isParryButtonPressed = false;
            Debug.Log("Shield down");
            ResetParryWindow();
        }
    }


    //When the player presses the shield icon 
    public void OnShield(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        //if there is no VR Player in the scene when Start() is called due to scene loading synchronisation
        if (!vrPlayer)
        {
            //find the VR Player in the scene
            vrPlayer = GameObject.FindGameObjectWithTag("VR_Player_Start").transform;

        }
    }

    //calls when shield button is pressed which begins the parry
    private void StartParryWindow()
    {
        if (parryAttackWindow != null)
        {
            StopCoroutine(parryAttackWindow);
        }
        parryAttackWindow = ParryWindowCoroutine();
        StartCoroutine(parryAttackWindow);
    }

    //IEnumerator used so that WaitForSeconds can be utilised for timing
    private IEnumerator ParryWindowCoroutine()
    {
        CallToggleServerRpc();
        isParryEnabled = true; //is only true for the duration of the parry window
        yield return new WaitForSeconds(parryWindow);
        ResetParryWindow(); //resets the parry window after the time is up
    }
    //End the parry
    private void ResetParryWindow()
    {
        //Debug.Log("Parry button released!");

        //makes sure any parry window on this player is stopped
        if (parryAttackWindow != null)
        {
            StopCoroutine(parryAttackWindow);
            parryAttackWindow = null;
            CallToggleServerRpc();

        }
        isParryEnabled = false;
        

    }

    //what to do when a bowling ball hits the player
    private void HandleAttack(bool canParry, bool canBlock, GameObject ball, Rigidbody brb)
    {
        if (!IsOwner) return;


        if (isParryEnabled && canParry) //happens when player released the shield button within the parry window
        {
            //send the ball back to the VR_Player when parrying the ball
            Debug.Log("Parried");

            NetworkObject ballNet = ball.GetComponent<NetworkObject>();

            ParryAttackServerRpc(ballNet);
        }
        else if (canBlock) //happens when the player holds the shield button down and just blocks
        {
            //when the player blocks, send them back a small amount
            Debug.Log("Block Performed");
            rb.velocity = new Vector3((force/4), 0f, 0f) + rb.velocity;
        }
        else //no parry and no block so player is launched
        {
            //when the player is hit, deactivate the ball, spawn particles and send the player flying
            DeactivateNetworkObjectServerRpc(ball);
            StartCoroutine(ConfettiSpawn(ball.transform.position));
            rb.velocity = new Vector3(force, 0f, 0f) + rb.velocity;
        }
    }


    
    void OnCollisionEnter(Collision other)
    {
        if (!IsOwner) return;

        //if the player was hit by a ball
        if (other.gameObject.tag == "Ball")
        {
            Rigidbody brb = other.gameObject.GetComponent<Rigidbody>();
            
            if (isParryButtonPressed)
            {
                HandleAttack(true, true, other.gameObject, brb);
            }
            else
            {
                HandleAttack(false, false, other.gameObject, brb);
            }
        }
    }

    //spawns a confetti vfx at the spot the ball collides with the pin
    //is a coroutine so that I can use the WaitForSeconds function
    IEnumerator ConfettiSpawn(Vector3 position)
    {
        NetworkObject confetti = ConfettiPool.SharedInstance.GetPooledObject();

        if (confetti != null)
        {
            confetti.transform.position = position;
            //effect would be played as it is set active
            ActivateNetworkObjectServerRpc(confetti);
            //one second would be enough as the effect happens on impact
            yield return new WaitForSeconds(1);
            //set active to false so that gameobject can be called again
            DeactivateNetworkObjectClientRpc(confetti);
        }
    }

    [ServerRpc]
    private void ParryAttackServerRpc(NetworkObjectReference ballRef)
    {
        ParryAttackClientRpc(ballRef);
    }

    [ClientRpc]
    private void ParryAttackClientRpc(NetworkObjectReference ballRef)
    {
        NetworkObject ballNet;
        ballRef.TryGet(out ballNet);

        Rigidbody ballRB = ballNet.GetComponent<Rigidbody>();

        vrPlayer = GameObject.FindGameObjectWithTag("VR_Player_Start").transform;
        ballNet.transform.LookAt(vrPlayer);

        ballRB.useGravity = false;
        ballRB.velocity = ballNet.transform.forward * 50;

    }


    [ServerRpc]
    private void CallToggleServerRpc()//Call on server first as clients cannot call a ClientRPC
    {
        ToggleShieldClientRpc();
    }



    [ClientRpc]
    private void ToggleShieldClientRpc() //Synchronise all clients with the shield object
    {
       
        parryShieldObj.SetActive(!parryShieldObj.activeSelf);
    }

    //call to every client from the server to deactivate a network object
    [ServerRpc]
    private void DeactivateNetworkObjectServerRpc(NetworkObjectReference obj)
    {
        DeactivateNetworkObjectClientRpc(obj);
    }

    //call to every client to deactivate a network object
    [ClientRpc]
    private void DeactivateNetworkObjectClientRpc(NetworkObjectReference obj)
    {
        NetworkObject objToDeac;

        obj.TryGet(out  objToDeac);

        objToDeac.gameObject.SetActive(false);
    }

    //call to every client from the server to activate a network object
    [ServerRpc]
    private void ActivateNetworkObjectServerRpc(NetworkObjectReference obj)
    {
        ActivateNetworkObjectClientRpc(obj);
    }

    //call to every client to activate a network object
    [ClientRpc]
    private void ActivateNetworkObjectClientRpc(NetworkObjectReference obj)
    {
        NetworkObject objToDeac;

        obj.TryGet(out objToDeac);

        objToDeac.gameObject.SetActive(false);
    }






}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;
using System;

public class MobileParry : NetworkBehaviour
{
    private IEnumerator parryAttackWindow;
    private bool isParryEnabled = false;
    private bool isParryWindowActive = false;
    [SerializeField] private float parryWindow;
    [SerializeField] private GameObject parryShieldObj;

    Rigidbody rb;
    float force = 20f;

    [SerializeField] private PlayerInput parryAction;
    
    [SerializeField] Transform vrPlayer;
    bool isParryButtonPressed;

    [SerializeField] private PlayerInput parryControls;

    private void Start()
    {
        if (!IsOwner) return;

        rb = GetComponent<Rigidbody>();

        //Deactivate shield visibility and find VR Player in the scene
        CallToggleServerRpc();
        vrPlayer = GameObject.FindGameObjectWithTag("VR_Player_Start").transform;
        parryAction = gameObject.GetComponent<PlayerInput>();
      
    }

    public void Update()
    {
        if (!IsOwner) return;

        if (parryAction.actions["Shield"].IsPressed() && !isParryButtonPressed)
        {
            isParryButtonPressed = true;
            Debug.Log("Shield up");
            StartParryWindow();

            
        }
        else if (!parryAction.actions["Shield"].IsPressed() && isParryButtonPressed)
        {
            isParryButtonPressed = false;
            Debug.Log("Shield down");
            ResetParryWindow();
            //CallToggleServerRpc();
            
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

        //if (context.started)
        //{
        //    Debug.Log("Shield up");
        //    StartParryWindow();
            
        //    isParryButtonPressed = true;
        //}
        //else if (context.canceled)
        //{
        //    Debug.Log("Shield down");
        //    ResetParryWindow();

        //    isParryButtonPressed = false;
        //}
    }

    private void StartParryWindow()
    {

        //Debug.Log("Parry button pressed!");

        if (parryAttackWindow != null)
        {
            StopCoroutine(parryAttackWindow);
        }
        parryAttackWindow = ParryWindowCoroutine();
        StartCoroutine(parryAttackWindow);
    }

    private IEnumerator ParryWindowCoroutine()
    {
        CallToggleServerRpc();
        isParryEnabled = true;
        isParryWindowActive = true;
        yield return new WaitForSeconds(parryWindow);
        ResetParryWindow();
    }

    private void ResetParryWindow()
    {
        //Debug.Log("Parry button released!");


        if (parryAttackWindow != null)
        {
            StopCoroutine(parryAttackWindow);
            parryAttackWindow = null;
            CallToggleServerRpc();

        }
        isParryEnabled = false;
        isParryWindowActive = false;
        

    }

    private void HandleAttack(bool canParry, bool canBlock, GameObject ball, Rigidbody brb)
    {
        if (!IsOwner) return;


        if (isParryEnabled && canParry)
        {
            Debug.Log("Parried");
            vrPlayer = GameObject.FindGameObjectWithTag("VR_Player_Start").transform;
            ball.transform.LookAt(vrPlayer);
            brb.useGravity = false;
            brb.velocity = ball.transform.forward * 50; //make force depend on distance as well //naw that was a bad idea
        }
        else if (canBlock)
        {
            Debug.Log("Block Performed");
            rb.velocity = new Vector3((force/4), 0f, 0f) + rb.velocity;
        }
        else
        {
            ball.SetActive(false);
            StartCoroutine(ConfettiSpawn(ball.transform.position));
            rb.velocity = new Vector3(force, 0f, 0f) + rb.velocity;
        }
    }


    
    void OnCollisionEnter(Collision other)
    {
        if (!IsOwner) return;


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
            confetti.gameObject.SetActive(true);
            //one second would be enough as the effect happens on impact
            yield return new WaitForSeconds(1);
            //set active to false so that gameobject can be called again
            confetti.gameObject.SetActive(false);
        }
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





}

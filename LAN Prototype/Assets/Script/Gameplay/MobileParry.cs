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
    
    [SerializeField] Transform vrPlayer;
    bool isParryButtonPressed;

    [SerializeField] private PlayerInput parryControls;

    private void Start()
    {
        if (!IsOwner) return;
        //Deactivate shield visibility and find VR Player in the scene
        ToggleShieldDeactiveClientRpc();
        vrPlayer = GameObject.FindGameObjectWithTag("VR_Player_Start").transform;
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

        if (context.started)
        {
            Debug.Log("Shield up");
            StartParryWindow();
            
            isParryButtonPressed = true;
        }
        if (context.canceled)
        {
            Debug.Log("Shield down");
            ResetParryWindow();

            isParryButtonPressed = false;
        }
    }

    private void StartParryWindow()
    {

        Debug.Log("Parry button pressed!");
        ToggleShieldActiveClientRpc();
        if (parryAttackWindow != null)
        {
            StopCoroutine(parryAttackWindow);
        }
        parryAttackWindow = ParryWindowCoroutine();
        StartCoroutine(parryAttackWindow);
    }

    private IEnumerator ParryWindowCoroutine()
    {
        isParryEnabled = true;
        isParryWindowActive = true;
        yield return new WaitForSeconds(parryWindow);
        ResetParryWindow();
    }

    private void ResetParryWindow()
    {
        Debug.Log("Parry button released!");
        ToggleShieldDeactiveClientRpc();

        if (parryAttackWindow != null)
        {
            StopCoroutine(parryAttackWindow);
            parryAttackWindow = null;
        }
        isParryEnabled = false;
        isParryWindowActive = false;
    }

    private bool IsParryWindowActive()
    {
        return isParryWindowActive;
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
        }
        else
        {
            ball.SetActive(false);
            StartCoroutine(ConfettiSpawn(ball.transform.position));
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

    [ClientRpc]
    private void ToggleShieldDeactiveClientRpc()
    {
        Debug.Log("Activate Shield");
        this.transform.GetChild(2).GetComponent<MeshRenderer>().enabled = false;
        parryShieldObj.SetActive(false);
    }

    [ClientRpc]
    private void ToggleShieldActiveClientRpc()
    {
        Debug.Log("Deactivate Shield");
        this.transform.GetChild(2).GetComponent<MeshRenderer>().enabled = true;
        parryShieldObj.SetActive(true);
    }



}

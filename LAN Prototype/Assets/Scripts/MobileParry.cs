using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MobileParry : NetworkBehaviour
{
    private IEnumerator parryAttackWindow;
    private bool isParryEnabled = false;
    private bool isParryWindowActive = false;
    [SerializeField] private float parryWindow;
    
    [SerializeField] Transform vrPlayer;
    bool isParryButtonPressed;

    private void Start()
    {
        vrPlayer = GameObject.FindGameObjectWithTag("VR_Player_Manager").GetComponent<VRPlayerManager>().GetCurrentPlayerTransform();
    }

    private void StartParryWindow()
    {
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

    private void HandleAttack(bool canParry, bool canBlock, GameObject ball, Rigidbody brb, Transform cannon)
    {
        if (isParryEnabled && canParry)
        {
            Debug.Log("Parried");
            ball.transform.LookAt(cannon);
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
        }
    }

    void Update()
    {
        if (Input.GetKeyDown("e")) //get mobile button
        {
            StartParryWindow();
            isParryButtonPressed = true;
        }
        if (Input.GetKeyUp("e")) //get mobile button
        {
            ResetParryWindow();
            isParryButtonPressed = false;
        }
    }
    
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "CannonBall")
        {
            Rigidbody brb = other.gameObject.GetComponent<Rigidbody>();
            
            if (isParryButtonPressed)
            {
                HandleAttack(true, true, other.gameObject, brb, vrPlayer);
            }
            else
            {
                HandleAttack(false, false, other.gameObject, brb, vrPlayer);
            }
        }
    }
}

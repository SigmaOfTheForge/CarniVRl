using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ParryManager : NetworkBehaviour //Control whether parry or just block is performed
{
    public static ParryManager SharedInstance;

    private IEnumerator parryAttackWindow;
    public bool isParryEnabled = false;
    private bool isParryWindowActive = false;
    [SerializeField] private float parryWindow;
    
    void Awake()
    {
        SharedInstance = this;
    }

    public void StartParryWindow()
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

    public void ResetParryWindow()
    {
        if (parryAttackWindow != null)
        {
            StopCoroutine(parryAttackWindow);
            parryAttackWindow = null;
        }
        isParryEnabled = false;
        isParryWindowActive = false;
    }

    public bool IsParryWindowActive()
    {
        return isParryWindowActive;
    }

    public void HandleAttack(bool canParry, bool canBlock, GameObject ball, Rigidbody brb, Transform cannon)
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
}

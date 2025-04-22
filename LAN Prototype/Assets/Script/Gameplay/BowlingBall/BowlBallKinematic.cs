using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BowlBallKinematic : MonoBehaviour
{
    //isKinematic was being set true when the bowling balls were enabled in the game level
    //this is the simplest solution
    void OnEnable()
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = false;


    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BallPit : ObjectPool
{
    //public static BallPit SharedInstance;
    
    void Awake()
    {
        //makes sure only the server can run the code
        if (Application.platform == RuntimePlatform.Android) return;

       // SharedInstance = this;
    }

    void Start()
    {
        //makes sure only the server can run the code
        if (Application.platform == RuntimePlatform.Android) return;


        ObjectInitiation();
    }
}

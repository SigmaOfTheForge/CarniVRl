using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BallPit : ObjectPool
{
    public static BallPit SharedInstance;
    
    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        ObjectInitiation();
    }
}

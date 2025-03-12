using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiPool : ObjectPool
{
    public static ConfettiPool SharedInstance;

    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        ObjectInitiation();
    }
}

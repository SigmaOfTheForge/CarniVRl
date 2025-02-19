using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BallPit : NetworkBehaviour
{
    public static BallPit SharedInstance;
    public List<NetworkObject> pooledObjects;
    public NetworkObject objectToPool;
    public int amountToPool;
    
    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        if (!IsHost) return;

        pooledObjects = new List<NetworkObject>();
        NetworkObject tmp;
        //instantiates the number of objects set and then
        //sets them as inactive before adding to pooled objects list
        //objects will be ready to use when the game runs
        for(int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool);
            tmp.Spawn();
            tmp.gameObject.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }

    //function so that other scripts can call it to use object in the pool
    //ie. set the object to active
    public NetworkObject GetPooledObject()
    {
        for(int i = 0; i < amountToPool; i++)
        {
            //when object is returned to the inactive state
            //it will be sent back to the pool
            if(!pooledObjects[i].gameObject.activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }
}

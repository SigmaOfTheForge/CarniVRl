using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

//is parent script that is inherited from
public class ObjectPool : NetworkBehaviour //MonoBehaviour -> NetworkBehaviour so that the script can have access to the network stuff
{
    public List<NetworkObject> pooledObjects; //GameObject -> NetworkObject for the same reason ^
    public NetworkObject objectToPool;
    public int amountToPool;

    public void ObjectInitiation()
    {
        //makes sure only the server can run the code
        if ( Application.platform == RuntimePlatform.Android ) return;
        
        pooledObjects = new List<NetworkObject>();
        NetworkObject tmp;
        //instantiates the number of objects set and then
        //sets them as inactive before adding to pooled objects list
        //objects will be ready to use when the game runs
        for (int i = 0; i < amountToPool; i++)
        {
            Debug.Log("SpawningObject");
            tmp = Instantiate(objectToPool); 
            tmp.Spawn();
            pooledObjects.Add(tmp);
            tmp.gameObject.SetActive(false);
            

        }
    }

    //function so that other scripts can call it to use object in the pool
    //ie. set the object to active
    public NetworkObject GetPooledObject()
    {
        if (pooledObjects.Count == 0) return null;
        
        for (int i = 0; i < amountToPool; i++)
        {
            //when object is returned to the inactive state
            //it will be sent back to the pool
            if (!pooledObjects[i].gameObject.activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }
}

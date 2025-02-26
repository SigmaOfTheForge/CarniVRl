using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDOLManager : MonoBehaviour
{
    public static DDOLManager instance;

    private List<GameObject> ddolObjects = new List<GameObject>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void AddDDOLObject(GameObject obj)
    {
        Debug.Log("Adding Object " + obj.name + " to DDOLManager");
        ddolObjects.Add(obj);
        DontDestroyOnLoad(obj);
        Debug.Log("Objects in DDOLManager: ");
        for (int i = 0; i < ddolObjects.Count; i++)
        {
            Debug.Log(ddolObjects[i].name);
        }
    }

    public void DeleteDDOLObjects()
    {
        Debug.Log("Deleting DDOL GameObjects");
        for(int i = ddolObjects.Count -1;  i >= 0; i--)
        {
            Debug.Log("Deleting GameObject: " + ddolObjects[i].name);
            Destroy(ddolObjects[i]);

        }

        Destroy(this);
    }


}

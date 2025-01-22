using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VrLaunchManager : MonoBehaviour
{
    [SerializeField]
    private Transform vrPlayerSpawnPoint, vrUISpawnPoint;

    [SerializeField]
    private GameObject vrPlayer, vrUI;


    // Start is called before the first frame update
    void Start()
    {
        GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
        Destroy(cam);
        Instantiate(vrPlayer);
        Instantiate(vrUI);
        Destroy(gameObject);
    }

}

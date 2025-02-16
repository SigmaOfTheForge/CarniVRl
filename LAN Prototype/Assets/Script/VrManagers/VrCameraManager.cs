using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class VrCameraManager : NetworkBehaviour 
{
    [SerializeField]
    private GameObject camToSpawn;

    private GameObject camSpawn;

    private void Awake()
    {
       
    }

    private void Start()
    {
        


        //camSpawn = Instantiate(camToSpawn, gameObject.transform);
        //this.GetComponentInParent<XROrigin>().SetCamera(camSpawn.GetComponent<Camera>());
        //this.GetComponentInParent<DynamicMoveProvider>().forwardSource = camSpawn.transform;
    }
}

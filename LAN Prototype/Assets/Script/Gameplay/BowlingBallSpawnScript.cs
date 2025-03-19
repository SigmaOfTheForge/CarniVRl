using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BowlingBallSpawnScript : NetworkBehaviour
{
    //NetworkObject bowlingBall;

    // Update is called once per frame
    void Update()
    {
        if (Application.platform == RuntimePlatform.Android) return;
 
        NetworkObject bowlingBall = BallPit.SharedInstance.GetPooledObject();

        if (bowlingBall != null)
        {
            Debug.Log("Bowling Ball set to active");
            bowlingBall.transform.position = this.transform.position;
            bowlingBall.transform.rotation = this.transform.rotation;
            bowlingBall.gameObject.SetActive(true);
        }
    }
}

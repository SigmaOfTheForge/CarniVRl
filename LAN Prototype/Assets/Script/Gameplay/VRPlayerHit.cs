using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPlayerHit : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            GameScoreManager.Instance.AddMobileScore(1);
            //Maybe also add some pp screen effects
            //like minecraft nausia
            //or a flash bang idk
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//goes on the barrier the mobile player hits after getting knocke outwadaw
public class MobilePlayerOut : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "mobilePlayer") 
        {
            other.gameObject.SetActive(false); //moblile player is killed
            ScoreCounter.SharedInstance.VRScored();
        }
        else
        {
            other.gameObject.SetActive(false);
        }
    }
}

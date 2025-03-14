using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//goes on the barrier the mobile player hits after getting knocke outwadaw
public class MobilePlayerOut : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MobilePlayer") 
        {
            other.gameObject.SetActive(false); //moblile player is killed
            GameScoreManager.Instance.AddVRScore(1);
        }
        else
        {
            other.gameObject.SetActive(false);
        }
    }
}

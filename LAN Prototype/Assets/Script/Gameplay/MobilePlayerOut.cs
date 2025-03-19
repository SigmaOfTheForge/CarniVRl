using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

//goes on the barrier the mobile player hits after getting knocke outwadaw
public class MobilePlayerOut : MonoBehaviour
{
    [SerializeField] private List<GameObject> mobileSpawns;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Mobile_Player") 
        {
            other.gameObject.SetActive(false); //moblile player is killed
            if(GameScoreManager.Instance != null) GameScoreManager.Instance.AddVRScore(1);
            RespawnPlayer(other);
        }
        else
        {
            other.gameObject.SetActive(false);
        }
    }


    private void RespawnPlayer(Collider other)
    {
        other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        other.transform.position = mobileSpawns[Random.Range(0, mobileSpawns.Count)].transform.position;
        other.gameObject.SetActive(true);
    }
}

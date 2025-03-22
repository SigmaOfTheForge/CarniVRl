using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering.Universal;

//goes on the barrier the mobile player hits after getting knocke outwadaw
public class MobilePlayerOut : NetworkBehaviour
{
    [SerializeField] private List<GameObject> mobileSpawns;

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;

        if (other.tag == "Mobile_Player") 
        {
            other.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false; //moblile player is "killed"
            other.SendMessage("ToggleMove");
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
        other.transform.position = mobileSpawns[Random.Range(0, mobileSpawns.Count -1)].transform.position;
        other.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        other.SendMessage("ToggleMove");

    }
}

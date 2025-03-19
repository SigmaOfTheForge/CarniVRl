using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class InactiveRespawnTest : MonoBehaviour
{
    [SerializeField] private GameObject assignedMobilePlayer;

    private void Update()
    {
        if (!assignedMobilePlayer.gameObject.activeSelf)
        {
            assignedMobilePlayer.transform.position = transform.position;
            assignedMobilePlayer.gameObject.SetActive(true);
        }
    }
}

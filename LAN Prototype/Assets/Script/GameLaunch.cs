using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLaunch : MonoBehaviour
{
    [SerializeField]
    private GameObject mobileUI, pcUI;


    //when the game begins, spawn in platform specific elements so they can start the game 
    private void Awake()
    {
        if(Application.platform == RuntimePlatform.Android)
        {
            Instantiate(mobileUI);
        }
        else if(Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            Instantiate(pcUI);
        }

        Destroy(gameObject);
    }

}

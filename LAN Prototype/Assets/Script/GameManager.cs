using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mobileUI, pcUI;

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
    }

}

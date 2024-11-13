using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableCamera : MonoBehaviour
{

    GameObject camObj;

    // Start is called before the first frame update
    void Start()
    {
        camObj = this.gameObject;
    }

    public void SwitchOffCamera()
    {
        camObj.SetActive(false);
    }
}

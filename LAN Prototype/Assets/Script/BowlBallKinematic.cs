using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlBallKinematic : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = false;

        
    }



}

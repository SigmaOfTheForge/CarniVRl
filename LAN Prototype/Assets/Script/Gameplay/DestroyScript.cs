using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyScript : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ball")
        {
            Destroy(gameObject);
        }
    }
}

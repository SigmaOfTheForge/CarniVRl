using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliForceCalculate : MonoBehaviour
{
    Rigidbody rb;
    float force = 20f;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ball")
        {
            rb.velocity = new Vector3(force, 0f, 0f) + rb.velocity;
        }
    }
}

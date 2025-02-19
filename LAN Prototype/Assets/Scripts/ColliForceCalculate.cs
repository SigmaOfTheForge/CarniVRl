using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliForceCalculate : MonoBehaviour
{
    Rigidbody rb;
    float force;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        force = 10f;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ball")
        {
            LaunchPlayer(other);
        }
    }

    void LaunchPlayer(Collision other)
    {
        rb.velocity = new Vector3 (0f, 0f, other.relativeVelocity.z * force) + rb.velocity;
    }
}

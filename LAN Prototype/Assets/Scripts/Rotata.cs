using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotata : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private bool isX, isY, isZ;

    void Update()
    {
        if(isX)
        {
            transform.Rotate(rotationSpeed * Time.deltaTime ,0.0f,  0.0f, Space.Self);
        }
        if(isY)
        {
            transform.Rotate(0.0f, rotationSpeed * Time.deltaTime , 0.0f, Space.Self);
        }
        if(isZ)
        {
            transform.Rotate(0.0f,  0.0f, rotationSpeed * Time.deltaTime ,Space.Self);
        }
        
    }
}

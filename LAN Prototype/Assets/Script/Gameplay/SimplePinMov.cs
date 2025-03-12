using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePinMov : MonoBehaviour
{
    [SerializeField] private float amplitude;
    [SerializeField] private float frequency;

    void Awake()
    {
        amplitude = Random.Range(0.01f, 0.02f);
        frequency = Random.Range(0.5f, 2f);
    }

    void Update()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x + (Mathf.Sin(Time.time * frequency) * amplitude), -1.3f, 1.3f), transform.position.y, transform.position.z);

    }
}

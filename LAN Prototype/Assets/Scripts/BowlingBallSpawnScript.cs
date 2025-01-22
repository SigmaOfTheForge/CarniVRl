using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingBallSpawnScript : MonoBehaviour
{
    [SerializeField] private GameObject bowlingBall;
    
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(bowlingBall, new Vector3(Random.Range(-1.5f, 1.5f), transform.position.y, transform.position.z), Quaternion.identity);
        Debug.Log(transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

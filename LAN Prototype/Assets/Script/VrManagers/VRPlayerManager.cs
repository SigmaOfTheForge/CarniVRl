using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;


public class VRPlayerManager : MonoBehaviour
{



    [SerializeField]
    private GameObject[] vrPlayerType;

    private GameObject currentPlayer;

    private Transform spawnPoint;

    private void Awake()
    {
        //clear any prior callback and then create a new one
        SceneManager.sceneLoaded -= OnLoadScene;
        SceneManager.sceneLoaded += OnLoadScene;

    }


    // Start is called before the first frame update
    void Start()
    {
        if(currentPlayer == null) 
        {
            spawnPoint = GameObject.FindGameObjectWithTag("VR_Spawn").transform;
            currentPlayer = Instantiate(vrPlayerType[0], spawnPoint.position, spawnPoint.rotation);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnLoadScene(Scene scene, LoadSceneMode mode)
    {
        int levelType = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelManager>().GetLevelType();
        if(currentPlayer != null)
        {
            Destroy(currentPlayer);
        }
        spawnPoint = GameObject.FindGameObjectWithTag("VR_Spawn").transform;
        currentPlayer = Instantiate(vrPlayerType[levelType], spawnPoint.position, spawnPoint.rotation);
        
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnScreenLog : MonoBehaviour
{
    uint qSize = 15;
    Queue myLogQueue = new Queue();

    private void Awake()
    {
        //clear any prior callback and then create a new one
        SceneManager.sceneLoaded -= OnLoadScene;
        SceneManager.sceneLoaded += OnLoadScene;
    }



    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Starting log Messages");
    }


    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }


    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        myLogQueue.Enqueue("["+ type + "] : " + logString);
        if(type == LogType.Exception)
        {
            myLogQueue.Enqueue(stackTrace);
        }
        while (myLogQueue.Count > qSize)
        {
            myLogQueue.Dequeue();
        }
    }

  void OnLoadScene(Scene scene, LoadSceneMode mode)
    {
        myLogQueue.Clear();
    }

    private void OnGUI()
    {
        GUI.backgroundColor = Color.black;
        GUILayout.BeginArea(new Rect(Screen.width - 400, 0, 400, Screen.height));
        
        GUILayout.Label("\n" + string.Join("\n", myLogQueue.ToArray()));
        GUILayout.EndArea();
    }

}

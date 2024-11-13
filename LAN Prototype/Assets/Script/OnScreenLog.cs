using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnScreenLog : MonoBehaviour
{
    uint qSize = 15;
    Queue myLogQueue = new Queue();

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


    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(Screen.width - 400, 0, 400, Screen.height));
        GUILayout.Label("\n" + string.Join("\n", myLogQueue.ToArray()));
        GUILayout.EndArea();
    }

}

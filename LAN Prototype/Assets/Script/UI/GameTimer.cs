using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class GameTimer : NetworkBehaviour
{
    //Maybe let people choose between time options.
    [SerializeField]
    float timer = 20f;

    [SerializeField]
    private TextMeshProUGUI[] timerText;
    private enum gameTimerState {Begin, Ongoing, End};

    private gameTimerState gameState;

    private float beginCountdown = 3;

    private bool gameEnded = false; 

    NetworkVariable<float> gameTimer = new NetworkVariable<float>();

    private void Start()
    {
        gameTimer.Value = timer;
        gameState = gameTimerState.Begin;
        
    }


    void Update()
    {
        //Idealy would be called when the game starts
        //After a 3 2 1 countdown or something like in mario cart after everyone is loaded in
        //looks like there is a tutorial for that which the timer function is controled by unity animator
        switch (gameState)
        {
            case gameTimerState.Begin:
                //countdown to game begin
                CountdownToStart();
                break;
            case gameTimerState.Ongoing:
                TimerCountdown();
                break;
            case gameTimerState.End:
                //goes to lobby scene
                GameEnd();
                break;
        }
    }

    void CountdownToStart()
    {
        beginCountdown -= Time.deltaTime;
        int simplifiedCount = Mathf.CeilToInt(beginCountdown);
        string updatedText;
        switch (simplifiedCount) 
        {
            case 3:
                updatedText = "3";
                foreach(TextMeshProUGUI i in timerText)
                {
                    i.text = updatedText;
                }
                break;
            case 2:
                updatedText = "2";
                foreach (TextMeshProUGUI i in timerText)
                {
                    i.text = updatedText;
                }
                break;
            case 1:
                updatedText = "1";
                foreach (TextMeshProUGUI i in timerText)
                {
                    i.text = updatedText;
                }
                break;
            case 0:
                updatedText = "Go";
                foreach (TextMeshProUGUI i in timerText)
                {
                    i.text = updatedText;
                }
                break;
            case -1:
                gameState = gameTimerState.Ongoing;
                break;
        }


    }
    
    void TimerCountdown()
    {
        //ENABLE WHEN TESTING IS OVER
        if (IsHost)
        {
            gameTimer.Value -= Time.deltaTime;

        }
        int seconds = Mathf.CeilToInt(gameTimer.Value);

        if (gameTimer.Value <= 0)
        {
            gameState = gameTimerState.End;
        }


        if (timerText != null )
        {
            string updatedText = seconds.ToString();

            foreach (TextMeshProUGUI boardText in timerText)
            {
                boardText.text = updatedText;
            }
        }
    }

    void GameEnd()
    {
        if (!gameEnded && IsHost)
        {
            gameEnded = true;

            StartCoroutine(GameEndedCoroutine());
        }
    }

    private IEnumerator GameEndedCoroutine()
    {
        string updatedText;

        updatedText = "Game Over";

        foreach (TextMeshProUGUI i in timerText)
        {
            i.text = updatedText;
        }

        yield return new WaitForSeconds(5);
        updatedText = "GOODBYE";

        foreach (TextMeshProUGUI i in timerText)
        {
            i.text = updatedText;
        }
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().ChangeScene("Lobby", 0);
    }
}

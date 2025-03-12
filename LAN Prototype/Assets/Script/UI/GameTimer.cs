using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    //Maybe let people choose between time options.
    float timer = 90.0f;
    public TextMeshProUGUI timerText;

    void Update()
    {
        //Idealy would be called when the game starts
        //After a 3 2 1 countdown or something like in mario cart after everyone is loaded in
        //looks like there is a tutorial for that which the timer function is controled by unity animator
        TimerCountdown();
    }

    void TimerCountdown()
    {
        timer -= Time.deltaTime;
        int seconds = Mathf.CeilToInt(timer);

        if (timerText != null )
        {
            timerText.text = seconds.ToString();
        }
    }
}

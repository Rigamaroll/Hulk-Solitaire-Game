using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    public static Timer instance;
    public Text timer;
    int min;
    int seconds;
    float time;
    int nextUpdate = 1;
    bool isTiming;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
     void Update()
    {
        if (isTiming){
            time += Time.deltaTime;
            min = (int)time / 60;
            seconds = (int)time % 60;
            if (seconds < 10)
            {
                timer.text = "Time: " + min + ":0" + seconds;
            }
            else
            {
                timer.text = "Time: " + min + ":" + seconds;
            }

            if (Time.time >= nextUpdate){
                nextUpdate = (int)time + 1;
                CheckScore();
            }
        }



    }

    private void CheckScore()
    {
        if ((int)time % 30 == 0 && (int)time > 0)
        {
            Scoring.instance.TimeReduceScore();

        }
    }

    public void StartScore()
    {
        isTiming = true;
    }

    public void StopScore()
    {
        isTiming = false;
    }
}

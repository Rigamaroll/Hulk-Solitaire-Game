using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    public Text timer;
    int min;
    int seconds;
    float time;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    private void FixedUpdate()
    {
        if ((int)time % 30 == 0 && (int)time > 0)
        {
            // Scoring.instance.TimeReduceScore();

        }
    }
}

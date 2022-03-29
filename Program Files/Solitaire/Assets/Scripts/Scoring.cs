using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoring : MonoBehaviour
{
    
    public Text points;
    public static Scoring instance;

    int score = 0;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        points.text = "Score: " + score.ToString(); //showing the score on the screen at the top
    }

    // Adding to the score value multiplier is how many times 5 score is reduced
    public void AddScore(int multiplier)
    {
        score += 5 * multiplier; // scoring value goes up
        points.text = "Score: " + score.ToString(); // updates the score value on the screen
    }

    //Reducing the score value, multiplier is how many times score reduced by 5
    public void ReduceScore(int multiplier)
    {
        score -= 5 * multiplier; // Scoring value goes down
        CheckScore();
        points.text = "Score: " + score.ToString(); // updates the score value on the scree
    }

    public void TimeReduceScore() // reducing the score every 30 seconds
    {
        score -= 2;
        CheckScore();
        points.text = "Score: " + score.ToString();
        
    }

    public void CheckScore() // checks the score so it doesn't go past 0
    {
        if (score < 0)
        {
            score = 0;
        }
    }

    // Resetting the score back to 0
    public void ResetScore()
    {
        score = 0; // score value sets back to 0
        points.text = "Score: " + score.ToString(); // updates the score value on the screen
    }
}

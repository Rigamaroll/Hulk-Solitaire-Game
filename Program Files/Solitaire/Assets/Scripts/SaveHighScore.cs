using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveHighScore : MonoBehaviour
{
    public static SaveHighScore instance;
    //Using text objects created in the High Score Canvas for Main Menu
    public Text sDeal1;
    public Text sDeal3;
    public Text vDeal1;
    public Text vDeal3;

    public int score = 0;
    public int highScore = 0;
    public int highScore1 = 0;
    public int highScore2 = 0;
    public int highScore3 = 0;
    public int highScore4 = 0;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        highScore1 = PlayerPrefs.GetInt("sDeal1HS", 0);
        highScore2 = PlayerPrefs.GetInt("sDeal3HS", 0);
        highScore3 = PlayerPrefs.GetInt("vDeal1HS", 0);
        highScore4 = PlayerPrefs.GetInt("vDeal3HS", 0);

        if (!MainMenu.GetOnVegas() && !MainMenu.GetDealThree()) //standard deal 1
        {
            PlayerPrefs.SetInt("sDeal1HS", Scoring.instance.updateScore); //if the current score is higher than previous high score then it will update the variable
        }
        if (!MainMenu.GetOnVegas() && MainMenu.GetDealThree())
        {
            PlayerPrefs.SetInt("sDeal3HS", Scoring.instance.updateScore); //if the current score is higher than previous high score then it will update the variable
        }
        if (MainMenu.GetOnVegas() && !MainMenu.GetDealThree())
        {
            PlayerPrefs.SetInt("vDeal1HS", Scoring.instance.updateScore); //if the current score is higher than previous high score then it will update the variable
        }
        if (MainMenu.GetOnVegas() && MainMenu.GetDealThree())
        {
            PlayerPrefs.SetInt("vDeal3HS", Scoring.instance.updateScore); //if the current score is higher than previous high score then it will update the variable
        }

        //sDeal1.text = "Standard - Deal 1: " + highScore.ToString();
        //sDeal3.text = "Standard - Deal 3: " + highScore.ToString();
        //vDeal1.text = "Vegas - Deal 1: " + highScore.ToString();
        //vDeal3.text = "Vegas - Deal 3: " + highScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CheckHighScore()
    {
        if (highScore1 < Scoring.instance.updateScore)
        {
            if (!MainMenu.GetOnVegas() && !MainMenu.GetDealThree()) //standard deal 1
            {

                PlayerPrefs.SetInt("sDeal1HS", Scoring.instance.updateScore); //if the current score is higher than previous high score then it will update the variable
            }
        }
        if (highScore2 < Scoring.instance.updateScore)
        {
            if (!MainMenu.GetOnVegas() && MainMenu.GetDealThree())
            {
                PlayerPrefs.SetInt("sDeal3HS", Scoring.instance.updateScore); //if the current score is higher than previous high score then it will update the variable
            }
        }
        if (highScore3 < Scoring.instance.updateScore)
        {
            if (MainMenu.GetOnVegas() && !MainMenu.GetDealThree())
            {
                PlayerPrefs.SetInt("vDeal1HS", Scoring.instance.updateScore); //if the current score is higher than previous high score then it will update the variable
            }
        }
        if (highScore4 < Scoring.instance.updateScore)
        {
            if (MainMenu.GetOnVegas() && MainMenu.GetDealThree())
            {
                PlayerPrefs.SetInt("vDeal3HS", Scoring.instance.updateScore); //if the current score is higher than previous high score then it will update the variable
            }
        }
    }
}

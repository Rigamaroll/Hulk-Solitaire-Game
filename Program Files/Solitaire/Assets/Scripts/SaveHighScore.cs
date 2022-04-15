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
    public int highScore3 = -52;
    public int highScore4 = -52;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        highScore1 = PlayerPrefs.GetInt("sDeal1HS", 0);
        highScore2 = PlayerPrefs.GetInt("sDeal3HS", 0);
        highScore3 = PlayerPrefs.GetInt("vDeal1HS", -52);
        highScore4 = PlayerPrefs.GetInt("vDeal3HS", -52);

        sDeal1.text = "Standard - Deal 1: " + highScore1.ToString();
        sDeal3.text = "Standard - Deal 3: " + highScore2.ToString();
        vDeal1.text = "Vegas - Deal 1: " + highScore3.ToString();
        vDeal3.text = "Vegas - Deal 3: " + highScore4.ToString();


        //******************DO NOT DELETE!!
        //if (!MainMenu.GetOnVegas() && !MainMenu.GetDealThree()) //standard deal 1
        //{
        //    highScore1 = PlayerPrefs.GetInt("sDeal1HS", 0);
        //    sDeal1.text = "Standard - Deal 1: " + highScore1.ToString();
        //}
        //if (!MainMenu.GetOnVegas() && MainMenu.GetDealThree())
        //{
        //    highScore2 = PlayerPrefs.GetInt("sDeal3HS", 0);
        //    sDeal3.text = "Standard - Deal 3: " + highScore2.ToString();
        //}
        //if (MainMenu.GetOnVegas() && !MainMenu.GetDealThree())
        //{
        //    highScore3 = PlayerPrefs.GetInt("vDeal1HS", 0);
        //    vDeal1.text = "Vegas - Deal 1: " + highScore3.ToString();
        //}
        //if (MainMenu.GetOnVegas() && MainMenu.GetDealThree())
        //{
        //    highScore4 = PlayerPrefs.GetInt("vDeal3HS", 0);
        //    vDeal3.text = "Vegas - Deal 3: " + highScore4.ToString();
        //    //PlayerPrefs.SetInt("vDeal3HS", Scoring.instance.updateScore); //if the current score is higher than previous high score then it will update the variable
        //}

    }

    public void CheckHighScore()
    {
        if (highScore1 < Scoring.instance.updateScore)
        {
            
            if (!MainMenu.GetOnVegas() && !MainMenu.GetDealThree()) //standard deal 1
            {
                Debug.Log("saving High Score for Standard Deal 1");
                PlayerPrefs.SetInt("sDeal1HS", Scoring.instance.updateScore); //if the current score is higher than previous high score then it will update the variable
            }
        }
        if (highScore2 < Scoring.instance.updateScore)
        {
            if (!MainMenu.GetOnVegas() && MainMenu.GetDealThree())
            {
                Debug.Log("saving High Score for Standard Deal 3");
                PlayerPrefs.SetInt("sDeal3HS", Scoring.instance.updateScore); //if the current score is higher than previous high score then it will update the variable
            }
        }
        if (highScore3 < Scoring.instance.updateScore)
        {
            if (MainMenu.GetOnVegas() && !MainMenu.GetDealThree())
            {
                Debug.Log("saving High Score for Vegas Deal 1");
                PlayerPrefs.SetInt("vDeal1HS", Scoring.instance.updateScore); //if the current score is higher than previous high score then it will update the variable
            }
        }
        if (highScore4 < Scoring.instance.updateScore)
        {
            if (MainMenu.GetOnVegas() && MainMenu.GetDealThree())
            {
                Debug.Log("saving High Score for Vegas Deal 3");
                PlayerPrefs.SetInt("vDeal3HS", Scoring.instance.updateScore); //if the current score is higher than previous high score then it will update the variable
            }
        }
    }
}

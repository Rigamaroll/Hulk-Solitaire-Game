using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMode : MonoBehaviour
{
    public Text gameMode;
    public static GameMode instance;

    private void Awake()
    {
        instance = this;
        
    }

    public void updateGameMode()
    {
        List<string> gameOptions = new List<string>(){ "Standard - Deal 1", "Standard - Deal 3", 
        "Vegas - Deal 1", "Vegas - Deal 3" };

        // print("made it");
        if (!MainMenu.GetOnVegas() && !MainMenu.GetDealThree())
        {
            gameMode.text = gameOptions[0];
        }
        if (!MainMenu.GetOnVegas() && MainMenu.GetDealThree())
        {
            gameMode.text = gameOptions[1];
        }
        if (MainMenu.GetOnVegas() && !MainMenu.GetDealThree())
        {
            gameMode.text = gameOptions[2];
        }
        if (MainMenu.GetOnVegas() && MainMenu.GetDealThree())
        {
            gameMode.text = gameOptions[3];
        }
    }
}

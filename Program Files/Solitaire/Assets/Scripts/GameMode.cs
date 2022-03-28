using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMode : MonoBehaviour
{
    public Text gameMode;
    public static GameMode instance;

    public void updateGameMode(int index)
    {
        List<string> gameOptions = new List<string>(){ "Standard - Deal 1", "Standard - Deal 3", 
        "Vegas - Deal 1", "Vegas - Deal 3" };

        gameMode.text = gameOptions[index];
        print("made it");
    }
}

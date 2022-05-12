using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] KeyCode pablosNewGame;
    [SerializeField] KeyCode pablosDone;
    [SerializeField] KeyCode pablosMenu;
    static bool isVegas = false;
    static bool isDealThree = false;
    // private GameMode gameMode;
    // public Text gameMode;

    public Dropdown dropdown;
    List<string> gameOptions = new List<string>(){ "Standard - Deal 1", "Standard - Deal 3", 
        "Vegas - Deal 1", "Vegas - Deal 3" };

    public void Start(){

        dropdown = GameObject.Find("GameOptions").GetComponent<Dropdown>();
        SetGameOptions();
        SetSelected();
        
    }
    public static void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
        //Debug.Log("Loading Game Scene");
        // gameMode.text = gameOptions[dropdown.value];
    }

    public static void QuitGame()
    {
        Application.Quit();
        //Debug.Log("Exiting App");
    }

    public static void HomeButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Add Key Entries
    public void FixedUpdate()
    {
        if (Input.GetKey(pablosNewGame)){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            if (MainMenu.GetOnVegas())
            {
                //Debug.Log("New Game: saving High Score for Vegas Deal");
                SaveHighScore.instance.CheckHighScore();
            }
        }
        else if(Input.GetKey(pablosMenu)){
            SceneManager.LoadScene("MainMenu");
            //Debug.Log("Exiting App");
            if (MainMenu.GetOnVegas())
            {
                //Debug.Log("New Game: saving High Score for Vegas Deal");
                SaveHighScore.instance.CheckHighScore();
            }            
        }
        else if(Input.GetKey(pablosDone)){
            Application.Quit();
            //Debug.Log("Exiting App");
        }
    }

    public void SetGameOptions()
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(gameOptions);
    }

    // Set the dropdown to the currently selected value (for when returning to the menu)
    public void SetSelected(){
        if (!isVegas && !isDealThree)
        {
            dropdown.value = 0;
            // gameMode.text = gameOptions[0];
        }
        if (!isVegas && isDealThree)
        {
            dropdown.value = 1;
            // gameMode.text = gameOptions[1];
        }
        if (isVegas && !isDealThree)
        {
            dropdown.value = 2;
            // gameMode.text = gameOptions[2];
        }
        if (isVegas && isDealThree)
        {
            dropdown.value = 3;
            // gameMode.text = gameOptions[3];
        }
        // gameMode.text = gameOptions[dropdown.value];
        // gameMode.updateGameMode(dropdown.value);
    }

    // Add Game Options
    public void GameOptionSelected(int index)
    {
        switch(index)
        {
            case 0:
                MainMenu.isVegas = false;
                isDealThree = false;
                break;
            case 1:
                MainMenu.isVegas = false;
                isDealThree = true;
                break;
            case 2:
                MainMenu.isVegas = true;
                isDealThree = false;
                break;
            case 3:
                MainMenu.isVegas = true;
                isDealThree = true;
                break;
        }

        // gameMode.updateGameMode(dropdown.value);
        // GameMode.instance.updateGameMode(index);
        // gameMode.text = gameOptions[index];
    }

    public static bool GetOnVegas(){
        return MainMenu.isVegas;
    }

    public static bool GetDealThree()
    {    
        return isDealThree;
    }
}

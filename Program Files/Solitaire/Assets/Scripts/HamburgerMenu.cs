using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class HamburgerMenu : MonoBehaviour
{
    public GameObject theHamburger;
    public Button Hamburger;
    public Button Menu;
    public Button NewGame;
    public Button Quit;
    bool isOpen = false;
    
    // public Button Resume;

    // Start is called before the first frame update
    void Awake()
    {
        theHamburger = GameObject.Find("HamburgerMenu");
        Hamburger = theHamburger.GetComponent<HamburgerMenu>().Hamburger;
        Menu = theHamburger.gameObject.GetComponent<HamburgerMenu>().Menu;
        NewGame = theHamburger.gameObject.GetComponent<HamburgerMenu>().NewGame;
        Quit = theHamburger.gameObject.GetComponent<HamburgerMenu>().Quit;

        Hamburger.gameObject.SetActive(true);
        Menu.gameObject.SetActive(false);
        NewGame.gameObject.SetActive(false);
        Quit.gameObject.SetActive(false);
    }

    private void Start()
    {
        Menu.gameObject.SetActive(true);
        NewGame.gameObject.SetActive(true);
        Quit.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenHamburgerMenu()
    {
        if (!isOpen)
        {
            Menu.gameObject.SetActive(true);
            NewGame.gameObject.SetActive(true);
            Quit.gameObject.SetActive(true);
            //Hamburger.gameObject.SetActive(false);
            PauseGame.instance.Pause();
        } else
        {
           // Hamburger.gameObject.SetActive(true);
            Menu.gameObject.SetActive(false);
            NewGame.gameObject.SetActive(false);
            Quit.gameObject.SetActive(false);
            PauseGame.instance.ResumeGame();

        }
        //print("opening hamburger menu");
        
        isOpen = !isOpen;
    }

    public void PressMainMenu()
    {
        //print("Pressed Main Menu");
        //***Test code for Vegas 3 and vegas deal 1
        if (MainMenu.GetOnVegas()) {
            //Debug.Log("Main Menu: saving High Score for Vegas Deal");
            SaveHighScore.instance.CheckHighScore();
        }
        SceneManager.LoadScene("MainMenu");
    }

    public void PressNewGame()
    {
        //print("Pressed New Game");
        //***Test code for Vegas 3 and vegas deal 1
        if (MainMenu.GetOnVegas())
        {
            //Debug.Log("New Game: saving High Score for Vegas Deal");
            SaveHighScore.instance.CheckHighScore();
        }
        SceneManager.LoadScene("GameScene");
    }

    public void PressQuit()
    {
        //print("Pressed Quit");
        Application.Quit();
    }

}

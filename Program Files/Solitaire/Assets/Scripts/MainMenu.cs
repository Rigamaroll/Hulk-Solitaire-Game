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

    public Dropdown dropdown;
    List<string> gameOptions = new List<string>(){ "Standard - Deal 1", "Standard - Deal 3", 
        "Vegas - Deal 1", "Vegas - Deal 3" };

    public void Start(){
        SetGameOptions();
        SetSelected();
    }
    public static void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
        Debug.Log("Loading Game Scene");
    }

    public static void QuitGame()
    {
        Application.Quit();
        Debug.Log("Exiting App");
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
        }
        else if(Input.GetKey(pablosMenu)){
            SceneManager.LoadScene("MainMenu");
            Debug.Log("Exiting App");
        }
        else if(Input.GetKey(pablosDone)){
            Application.Quit();
            Debug.Log("Exiting App");
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
        }
        if (!isVegas && isDealThree)
        {
            dropdown.value = 1;
        }
        if (isVegas && !isDealThree)
        {
            dropdown.value = 2;
        }
        if (isVegas && isDealThree)
        {
            dropdown.value = 3;
        }
    }

    // Add Game Options
    public void GameOptionSelected(int index)
    {
        if (index == 0)
        {
            print("Standard 1");
            MainMenu.isVegas = false;
            isDealThree = false;
        }
        if (index == 1)
        {
            print("Standard 3");
            MainMenu.isVegas = false;
            isDealThree = true;
        }
        if (index == 2)
        {
            print("Vegas 1");
            MainMenu.isVegas = true;
            isDealThree = false;
        }
        if (index == 3)
        {
            print("Vegas 3");
            MainMenu.isVegas = true;
            isDealThree = true;
        }
    }

    public static void OnStandard(){
        MainMenu.isVegas = false;
        print("isVegas is set to: " + MainMenu.isVegas);
    }

    public static void OnVegas(){
        MainMenu.isVegas = true;
        print("isVegas is set to: " + MainMenu.isVegas);
    }

    public static bool GetOnVegas(){
        return MainMenu.isVegas;
    }

    public static void OnDealThree()
    {
        print("Dealing three Cards");
        isDealThree = true;
    }

    public static void OnDealOne()
    {
        print("Dealing One Card");
        isDealThree = false;
    }

    public static bool GetDealThree()
    {    
        return isDealThree;
    }
}

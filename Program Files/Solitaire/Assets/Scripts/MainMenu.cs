using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] KeyCode pablosNewGame;
    [SerializeField] KeyCode pablosDone;
    [SerializeField] KeyCode pablosMenu;
    static bool isVegas = false;

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
}

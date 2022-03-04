using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] KeyCode pablosNewGame;
    [SerializeField] KeyCode pablosDone;
    [SerializeField] KeyCode pablosMenu;
    private bool isVegas = false;

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
        Debug.Log("Loading Game Scene");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Exiting App");
    }

    public void HomeButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Add Key Entries
    void FixedUpdate()
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

    public void OnStandard(){
        isVegas = false;
        print("isVegas is set to: " + isVegas);
    }

    public void OnVegas(){
        isVegas = true;
        print("isVegas is set to: " + isVegas);
    }

    public bool GetOnVegas(){
        return isVegas;
    }
}

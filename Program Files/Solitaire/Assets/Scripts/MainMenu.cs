using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] KeyCode pablosNewGame;
    [SerializeField] KeyCode pablosDone;
    private bool isVegas;

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
        else if(Input.GetKey(pablosDone)){
            Application.Quit();
            Debug.Log("Exiting App");
        }
    }

    public void onStandard(){
        isVegas = false;
        print("isVegas is set to: " + isVegas);
    }

    public void onVegas(){
        isVegas = true;
        print("isVegas is set to: " + isVegas);
    }

    public bool getOnVegas(){
        return isVegas;
    }
}

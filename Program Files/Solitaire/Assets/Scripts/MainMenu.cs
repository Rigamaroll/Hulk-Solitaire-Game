using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
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
}

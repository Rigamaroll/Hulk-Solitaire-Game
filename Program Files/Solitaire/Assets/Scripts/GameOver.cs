using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public static GameOver instance;
    public GameObject gameOverUI; //Game object for Game Over Panel

    private void Awake()
    {
        instance = this;
    }

    //method to display the game over screen
    public void GameOverScreen()
    {
        gameOverUI.SetActive(true); //loads the game over screen
        SaveHighScore.instance.CheckHighScore(); //Updates the high score
    }

    //method to load the new game
    public void RestartGame()
    {
        //Debug.Log("Pressed New Game"); //print out statement for testing
        gameOverUI.SetActive(false); //unloads the game over screen
        SceneManager.LoadScene("GameScene"); //loads the Game Scene
        /*** TEST ***/
        //SaveHighScore.instance.CheckHighScore(); //updates the high score 
    }

    //method to load the main menu screen
    public void MainMenuScreen()
    {
        //Debug.Log("Pressed Main Menu"); //print out statement for testing
        gameOverUI.SetActive(false); //unloads the game over screen
        SceneManager.LoadScene("MainMenu"); //loads the Main Menu game scene
        /*** TEST ***/
        //SaveHighScore.instance.CheckHighScore(); //updates the high score
    }

    //method to quit the game
    public void QuitGame()
    {
        //Debug.Log("Quitting Game...");
        Application.Quit(); //quits the application
    }
}

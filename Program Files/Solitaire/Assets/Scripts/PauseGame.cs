using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    public Button Resume;
    public GameObject panel;
    public static PauseGame instance;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        Time.timeScale = 1f;
        Resume.gameObject.SetActive(false);
        panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        Resume.gameObject.SetActive(true);
        panel.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        Resume.gameObject.SetActive(false);
        panel.SetActive(false);
        
    }
}

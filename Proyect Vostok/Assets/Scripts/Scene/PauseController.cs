using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public Button menuButton;

    private bool isPaused = false;

    void Start()
    {
        pauseMenuUI.SetActive(false);
        menuButton.onClick.AddListener(OnContinueButton);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)&&!pauseMenuUI.activeSelf)
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame(); 
            }
        }
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; 
        isPaused = false;
    }

    public void OnContinueButton()
    {
        ResumeGame();
    }
}

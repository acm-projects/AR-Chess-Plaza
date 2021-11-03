using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    public void PauseGame()
    {
        GameIsPaused = true;
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
    }

    public void ResumeGame()
    {
        GameIsPaused = false;
        Time.timeScale = 1.0f;
        pauseMenuUI.SetActive(false);

    }
}

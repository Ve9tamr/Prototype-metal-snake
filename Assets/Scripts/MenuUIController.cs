using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIController : MonoBehaviour
{

    public static bool GameIsPaused;
    public GameObject PauseMenuUI;
    public GameObject TutorialMenuUI;
    public GameObject WinMenuUI;
    public GameObject LoseMenuUI;

    private void Awake()
    {
        PauseMenuUI.SetActive(false);
        WinMenuUI.SetActive(false);
        LoseMenuUI.SetActive(false);
        TutorialMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        TutorialMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void GameWin()
    {
        WinMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void GameLose()
    {
        LoseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
}

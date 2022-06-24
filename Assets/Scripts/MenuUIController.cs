using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUIController : MonoBehaviour
{

    public static bool GameIsPaused;
    public GameObject PauseMenuUI;
    public GameObject TutorialMenuUI1;
    public GameObject TutorialMenuUI2;
    public GameObject TutorialMenuUI3;
    public GameObject WinMenuUI;
    public GameObject LoseMenuUI;

    private void Awake()
    {
        PauseMenuUI.SetActive(false);
        WinMenuUI.SetActive(false);
        LoseMenuUI.SetActive(false);
        TutorialMenuUI2.SetActive(false);
        TutorialMenuUI3.SetActive(false);
        TutorialMenuUI1.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        TutorialMenuUI1.SetActive(false);
        TutorialMenuUI2.SetActive(false);
        TutorialMenuUI3.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Tutorial2()
    {
        TutorialMenuUI2.SetActive(true);
        TutorialMenuUI1.SetActive(false);
    }

    public void Tutorial3()
    {
        TutorialMenuUI3.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
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
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}

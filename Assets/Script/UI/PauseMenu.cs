using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * This module satisfies:
 *   - Functional requirement 2.8
 *   - Functional requirement 4.1
 *   - Functional requirement 4.3
 */
public class PauseMenu : MonoBehaviour
{
    public static bool GamePaused = false;
    public GameObject PauseMenuUI;
    public GameObject WarningMenuUI;
    public GameObject QuitGameUI;
    public GameObject BarsUI;

    // Update is called once per frame
    void Update()
    {
        // Escape or Return Key is responsible for going to and from pause menu
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return))
        {
            if (GamePaused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);  // Set Pause menu UI to invisible
        BarsUI.SetActive(true);  // Show the health and progress bars
        Time.timeScale = 1f; // Continue game running
        GamePaused = false;  // Bool for keeping track of status
    }

    public void Pause()
    {
        PauseMenuUI.SetActive(true);  // Set Pause menu UI to visible
        BarsUI.SetActive(false);  // Hide the health and progress bars
        Time.timeScale = 0f; // Freeze the game
        GamePaused = true;  // Bool for keeping track of status
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void MainMenuWarning()
    {
        PauseMenuUI.SetActive(false);  // Set Pause menu UI to invisible
        WarningMenuUI.SetActive(true);  // Set warning menu to visible
    }

    public void QuitWarning()
    {
        PauseMenuUI.SetActive(false);  // Set Pause menu UI to invisible
        QuitGameUI.SetActive(true);  // Set warning menu to visible
    }

    public void ReturnSettings()
    {
        PauseMenuUI.SetActive(true);  // Set Pause menu UI to visible
        WarningMenuUI.SetActive(false);  // Set warning menu to invisible
        QuitGameUI.SetActive(false);  // Set warning menu to invisible
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;  // Unpause game before leaving
        SceneManager.LoadScene(sceneBuildIndex: 0);
    }
}

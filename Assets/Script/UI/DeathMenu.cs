using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * This module satisfies:
 *   - Functional requirement 5.1
 *   - Functional requirement 5.2
 */
public class DeathMenu : MonoBehaviour
{
    public GameObject BarsUI;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f; // Freeze the game
        BarsUI.SetActive(false); // Hide health and progress bar
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;  // Unpause game before leaving
        SceneManager.LoadScene(sceneBuildIndex: 0);
    }

    public void Restart()
    {
        if (PlayerPrefs.GetString("mode") == "easy")
        {
            PlayerPrefs.SetInt("health", 100); // reset health
            PlayerPrefs.Save();
            Time.timeScale = 1f;  // Unpause game before leaving
            BarsUI.SetActive(true);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        } else
        {
            PlayerPrefs.SetInt("health", 100); // reset health
            PlayerPrefs.Save();
            Time.timeScale = 1f;  // Unpause game before leaving
            SceneManager.LoadScene("Level 1");
        }
    }
}

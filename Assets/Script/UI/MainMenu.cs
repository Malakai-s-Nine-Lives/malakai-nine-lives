using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public void Start()
    {
        // set the default play mode to easy
        PlayerPrefs.SetString("mode", "easy");
        PlayerPrefs.SetInt("health", 100); // start off with 100 health
        PlayerPrefs.Save();
    }

    public void PlayGame()  // Load first level if play
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()  // Quit game
    {
        Debug.Log("Quitted!");
        Application.Quit();
    }
}

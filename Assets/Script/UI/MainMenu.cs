using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private int _easy_mode = 1;  // Store the mode we are playing

    public int easy_mode
    {
        get { return _easy_mode; }
        set { _easy_mode = value; }
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

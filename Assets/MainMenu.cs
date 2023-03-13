using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private int _easy_mode = 1;
    public Button easy_mode_button;
    public Button hard_mode_button;

    public int easy_mode
    {
        get { return _easy_mode; }
        set { _easy_mode = value; }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Quitted!");
        Application.Quit();
    }

    public void SettingsButton()
    {
        if (easy_mode == 1)
        {
            easy_mode_button.Select();
        }
        else
        {
            hard_mode_button.Select();
        }
    }
}

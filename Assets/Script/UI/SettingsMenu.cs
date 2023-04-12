using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * This module satisfies:
 *   - Functional requirement 1.2
 */
public class SettingsMenu : MonoBehaviour
{
    public MainMenu script;
    public TMP_Text mode_text;

    public void HardMode()  // If we select hard mode
    {
        mode_text.text = "Hard";
        PlayerPrefs.SetString("mode", "hard");
        PlayerPrefs.SetInt("health", 100); // start off with 100 health
        PlayerPrefs.Save();
    }

    public void EasyMode()  // If we select Easy mode
    {
        mode_text.text = "Easy";
        PlayerPrefs.SetString("mode", "easy");
        PlayerPrefs.SetInt("health", 100); // start off with 100 health
        PlayerPrefs.Save();
    }
}

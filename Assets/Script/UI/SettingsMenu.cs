using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] Slider volume_slider;
    public MainMenu script;
    public TMP_Text mode_text;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetFloat("musicVolume", 1);
    }

    public void changeVolume()  // Change the volume according to slider
    {
        AudioListener.volume = volume_slider.value;
    }

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

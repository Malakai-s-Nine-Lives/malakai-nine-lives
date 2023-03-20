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
        script.easy_mode = 0;
        mode_text.text = "Hard";
    }

    public void EasyMode()  // If we select Easy mode
    {
        script.easy_mode = 1;
        mode_text.text = "Easy";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] Slider volume_slider;
    public MainMenu script;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetFloat("musicVolume", 1);
    }

    public void changeVolume()
    {
        AudioListener.volume = volume_slider.value;
    }

    public void HardMode()
    {
        script.easy_mode = 0;
        Debug.Log(script.easy_mode);
    }

    public void EasyMode()
    {
        script.easy_mode = 1;
        Debug.Log(script.easy_mode);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public Slider volumeSlider;
    // Start is called before the first frame update
    void Start()
    {
        // Check if the key exists
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolume();  // Load the volume we have saved
        } else
        {
            volumeSlider.value = 1;  // Otherwise assign the volume as 100%
            SaveVolume();
        }
    }

    public void ChangeVolume()
    {
        // Take the slider value and assign it as the volume value
        AudioListener.volume = volumeSlider.value;
        SaveVolume();
    }

    private void LoadVolume()
    {
        // Load the volume form Player Prefs
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");

    }

    private void SaveVolume()
    {
        // Save the volume in Player Prefs
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
}

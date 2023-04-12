using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This module satisfies:
 *   - Functional requirement 1.2
 */
public class LevelAudio : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Play the stored version of the volume
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            AudioListener.volume = PlayerPrefs.GetFloat("musicVolume");
        }
        else  // Store the volume as 100% if not stored
        {
            AudioListener.volume = 1;
            PlayerPrefs.SetFloat("musicVolume", 1);

        }
    }
}

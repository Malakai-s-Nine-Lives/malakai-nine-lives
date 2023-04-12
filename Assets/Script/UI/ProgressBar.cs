using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * This module satisfies:
 *   - Functional requirement 2.7
 *   - Functional requirement 8.1
 *   - Functional requirement 9.1
 */
public class ProgressBar : MonoBehaviour
{
    public Slider slider;
    public void SetMaxPoints(int maxPoints)
    {
        slider.maxValue = maxPoints;
        slider.value = 0;
    }
    public void SetPoints(int points)
    {
        slider.value = points;
    }
}


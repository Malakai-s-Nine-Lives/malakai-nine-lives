using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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


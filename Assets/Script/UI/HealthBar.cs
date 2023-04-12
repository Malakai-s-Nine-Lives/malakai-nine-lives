using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * This module help satisfy:
 *   - Functional requirement 2.5
 *   - Functional requirement 5.1
 */
public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }
    public void SetHealth(int health)
    {
        slider.value = health;
    }
}

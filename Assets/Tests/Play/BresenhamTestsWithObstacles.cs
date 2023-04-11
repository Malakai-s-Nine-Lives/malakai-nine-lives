using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class BresenhamTestsWithObstacles: BresenhamTests
{

    [OneTimeSetUp]
    public void LoadBresenhamTestScene()
    {
        SceneManager.LoadScene("BresenhamTestWithObstacles");
    }


    [UnityTest]
    public IEnumerator BresenhamWithObstaclesUpInvalid()
    {
        Vector3 targetVector = new Vector3(-7, 0, 0);
        bool bresenhamResult = Bresenham.determineActivation(freeSightChance, sightRadius, originVector, targetVector);
        Assert.IsFalse(bresenhamResult);
        bresenhamResult = Bresenham.determineActivation(freeSightChance, sightRadius, targetVector, originVector);
        Assert.IsFalse(bresenhamResult);
        yield return null;
    }

    [UnityTest]
    public IEnumerator BresenhamWithObstaclesRightInvalid()
    {
        Vector3 targetVector = new Vector3(-3, -4, 0);
        bool bresenhamResult = Bresenham.determineActivation(freeSightChance, sightRadius, originVector, targetVector);
        Assert.IsFalse(bresenhamResult);
        bresenhamResult = Bresenham.determineActivation(freeSightChance, sightRadius, targetVector, originVector);
        Assert.IsFalse(bresenhamResult);
        yield return null;
    }


    [UnityTest]
    public IEnumerator BresenhamWithObstaclesDiagonaInvalid()
    {
        Vector3 targetVector = new Vector3(-3, -1, 0);
        bool bresenhamResult = Bresenham.determineActivation(freeSightChance, sightRadius, originVector, targetVector);
        Assert.IsFalse(bresenhamResult);
        bresenhamResult = Bresenham.determineActivation(freeSightChance, sightRadius, targetVector, originVector);
        Assert.IsFalse(bresenhamResult);
        yield return null;
    }

    [UnityTest]
    public IEnumerator BresenhamWithObstaclesDiagonalQ2ToQ4()
    {
        Vector3 upper_left = new Vector3(-7, -1, 0);
        Vector3 lower_right = new Vector3(-3, -4, 0);
        bool bresenhamResult = Bresenham.determineActivation(freeSightChance, sightRadius, upper_left, lower_right);
        Assert.IsFalse(bresenhamResult);
        bresenhamResult = Bresenham.determineActivation(freeSightChance, sightRadius, lower_right, upper_left);
        Assert.IsFalse(bresenhamResult);
        yield return null;
    }
}

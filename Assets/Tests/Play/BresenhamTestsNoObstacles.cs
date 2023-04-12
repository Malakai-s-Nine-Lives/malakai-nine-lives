using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class BresenhamTestsNoObstacles: BresenhamTests
{

    [OneTimeSetUp]
    public void LoadBresenhamTestScene()
    {
        SceneManager.LoadScene("BresenhamTestNoObstacles");
    }

    [UnityTest]
    public IEnumerator BresenhamNoObstaclesUpValid()
    {
        Vector3 targetVector = new Vector3(-7, 0, 0);
        bool bresenhamResult = Bresenham.determineActivation(freeSightChance, sightRadius, originVector, targetVector);
        Assert.IsTrue(bresenhamResult);
        bresenhamResult = Bresenham.determineActivation(freeSightChance, sightRadius, targetVector, originVector);
        Assert.IsTrue(bresenhamResult);
        yield return null;
    }

    [UnityTest]
    public IEnumerator BresenhamNoObstaclesUpValidButTooFar()
    {
        Vector3 targetVector = new Vector3(-7, 4, 0);
        bool bresenhamResult = Bresenham.determineActivation(freeSightChance, sightRadius, originVector, targetVector);
        Assert.IsFalse(bresenhamResult);
        bresenhamResult = Bresenham.determineActivation(freeSightChance, sightRadius, targetVector, originVector);
        Assert.IsFalse(bresenhamResult);
        yield return null;
    }

    [UnityTest]
    public IEnumerator BresenhamNoObstaclesRightValid()
    {
        Vector3 targetVector = new Vector3(-3, -4, 0);
        bool bresenhamResult = Bresenham.determineActivation(freeSightChance, sightRadius, originVector, targetVector);
        Assert.IsTrue(bresenhamResult);
        bresenhamResult = Bresenham.determineActivation(freeSightChance, sightRadius, targetVector, originVector);
        Assert.IsTrue(bresenhamResult);
        yield return null;
    }

    [UnityTest]
    public IEnumerator BresenhamNoObstaclesRightValidButTooFar()
    {
        Vector3 targetVector = new Vector3(8, -4, 0);
        bool bresenhamResult = Bresenham.determineActivation(freeSightChance, sightRadius, originVector, targetVector);
        Assert.IsFalse(bresenhamResult);
        bresenhamResult = Bresenham.determineActivation(freeSightChance, sightRadius, targetVector, originVector);
        Assert.IsFalse(bresenhamResult);
        yield return null;
    }

    [UnityTest]
    public IEnumerator BresenhamNoObstaclesDiagonalValid()
    {
        Vector3 targetVector = new Vector3(-3, -1, 0);
        bool bresenhamResult = Bresenham.determineActivation(freeSightChance, sightRadius, originVector, targetVector);
        Assert.IsTrue(bresenhamResult);
        bresenhamResult = Bresenham.determineActivation(freeSightChance, sightRadius, targetVector, originVector);
        Assert.IsTrue(bresenhamResult);
        yield return null;
    }

    [UnityTest]
    public IEnumerator BresenhamNoObstaclesDiagonalInvalidAndTooFar()
    {
        Vector3 targetVector = new Vector3(8, 4, 0);
        bool bresenhamResult = Bresenham.determineActivation(freeSightChance, sightRadius, originVector, targetVector);
        Assert.IsFalse(bresenhamResult);
        bresenhamResult = Bresenham.determineActivation(freeSightChance, sightRadius, targetVector, originVector);
        Assert.IsFalse(bresenhamResult);
        yield return null;
    }

    [UnityTest]
    public IEnumerator BresenhamNoObstaclesDiagonalQ2ToQ4()
    {
        Vector3 upper_left = new Vector3(-7, -1, 0);
        Vector3 lower_right = new Vector3(-3, -4, 0);
        bool bresenhamResult = Bresenham.determineActivation(freeSightChance, sightRadius, upper_left, lower_right);
        Assert.IsTrue(bresenhamResult);
        bresenhamResult = Bresenham.determineActivation(freeSightChance, sightRadius, lower_right, upper_left);
        Assert.IsTrue(bresenhamResult);
        yield return null;
    }

}

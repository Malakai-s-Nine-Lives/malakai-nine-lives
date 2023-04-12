using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
// using UnityEngine.SceneMode;

public class BresenhamTests
{
    // Bresenham needs to run deterministically for the tests
    protected int freeSightChance = 0;
    // default sight chance for the game
    protected int sightRadius = 5;
    // origin vector for all tests
    protected Vector3 originVector = new Vector3(-7, -4, 0);
    
    [UnitySetUp]
    public IEnumerator WaitForGameObjectsToLoad()
    {
        yield return new WaitForSeconds(1);
    }
}

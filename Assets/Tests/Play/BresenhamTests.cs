using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
// using UnityEngine.SceneMode;

public class BresenhamTests
{
    // A Test behaves as an ordinary method
    [UnityTest]
    public IEnumerator BresenhamTestsValid()
    {
        SceneManager.LoadScene("BresenhamTestValid");
        // let the other game objects start
        yield return new WaitForSeconds(3);
        // grab the enemy patroller
        PlatformPatrollingEnemy platformPatroller = GameObject.Find("BasicEnemy").GetComponent<PlatformPatrollingEnemy>();
        
        // float startTime = Time.time;
        while (Time.time < 5)
        {
            // let the enemy see malakai first
            yield return null;
            // the enemy should always be activated
            Assert.IsTrue(platformPatroller.isActivated());
        }
        
    }

        // A Test behaves as an ordinary method
    [UnityTest]
    public IEnumerator BresenhamTestsInvalid()
    {
        SceneManager.LoadScene("BresenhamTestInvalid");
        // let the other game objects start
        yield return new WaitForSeconds(3);
        // grab the enemy patroller
        PlatformPatrollingEnemy platformPatroller = GameObject.Find("BasicEnemy").GetComponent<PlatformPatrollingEnemy>();
        
        // run the test for 5 seconds
        while (Time.time < 5)
        {
            // let the enemy see malakai first
            yield return null;
            // the enemy should always be activated
            Assert.IsFalse(platformPatroller.isActivated());
        }

    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    // [UnityTest]
    // public IEnumerator BresenhamTestsWithEnumeratorPasses()
    // {
    //     // Use the Assert class to test conditions.
    //     // Use yield to skip a frame.
    //     yield return null;
    // }

    
}

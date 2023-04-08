using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class Edit1
{
    // A Test behaves as an ordinary method
    [Test]
    public void Edit1SimplePasses()
    {
        // Use the Assert class to test conditions
        Assert.AreEqual(2, 1+1);
    }

    

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator Edit1WithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}

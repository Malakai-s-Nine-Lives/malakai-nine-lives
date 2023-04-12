using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class MapGridTests
{
    public static MapGrid grid;
    
    [OneTimeSetUp]
    public void LoadAStarScene()
    {
        SceneManager.LoadScene("AStarTest");
    }
    
    [UnitySetUp]
    public IEnumerator WaitForGameObjectsToLoad()
    {
        yield return new WaitForSeconds(1);
        grid = GameObject.Find("Algorithms").GetComponent<MapGrid>();
    }

    [UnityTest]
    public IEnumerator MapGridTestsNonWalkableNode()
    {
        Node node = grid.NodeFromMapPoint(new Vector2(-1.6f,-1.0f));
        Assert.IsFalse(node.walkable);
        yield return null;
    }
    
    [UnityTest]
    public IEnumerator MapGridTestsWalkableNode()
    {
        Node node = grid.NodeFromMapPoint(new Vector2(-2.0f,-1f));
        Assert.IsTrue(node.walkable);
        yield return null;
    }

    [UnityTest]
    public IEnumerator MapGridTestsSize()
    {
        // the gridsize for this scene is 1980
        Assert.AreEqual(grid.MaxSize, 1980);
        yield return null;
    }
}

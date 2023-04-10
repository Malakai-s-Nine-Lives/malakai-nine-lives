using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class AStarTests
{
    private static Camera camera;
    public static MapGrid grid;
    public static AStar aStar;
    
    [OneTimeSetUp]
    public void LoadAStarScene()
    {
        Debug.Log("Loading the AStarTest scene");
        SceneManager.LoadScene("AStarTest");
        // wait for the scene to load
    }
    
    [UnitySetUp]
    public IEnumerator WaitForGameObjectsToLoad()
    {
        Debug.Log("AStarTest: wait for game objects to load");
        yield return new WaitForSeconds(1);
        grid = GameObject.Find("Algorithms").GetComponent<MapGrid>();
        aStar = GameObject.Find("Algorithms").GetComponent<AStar>();
        camera = Camera.main;
    }

    [UnityTest]
    public IEnumerator AStarMultipleRequests()
    {
        Vector2 source = new Vector2(-7, -4);
        Vector2 destination = new Vector2(0, -1);
        Vector2 invalidDestination = new Vector2(5, 2);
        // Validate that a path IS found
        void OnPathFound(Vector2[] newPath, bool pathSuccessful)
        {
            // this test should not crash the system
            Assert.IsTrue(true);
        }
        PathRequest.RequestPath(source, destination, OnPathFound);
        PathRequest.RequestPath(source, invalidDestination, OnPathFound);
        PathRequest.RequestPath(source, destination, OnPathFound);
        PathRequest.RequestPath(source, invalidDestination, OnPathFound);
        PathRequest.RequestPath(source, destination, OnPathFound);
        // let the A star algorithm run
        yield return new WaitForSeconds(5);
    }

    [UnityTest]
    public IEnumerator AStarBasicTest()
    {
        Vector2 source = new Vector2(-7, -4);
        Vector2 destination = new Vector2(0, -1);
        // Validate that a path IS found
        void OnPathFound(Vector2[] newPath, bool pathSuccessful)
        {
            Assert.IsTrue(pathSuccessful);
            Node node;
            foreach (Vector2 onePath in newPath)
            {
                // obtain the node from a path
                // node = grid.NodeFromMapPoint(camera.ScreenToWorldPoint(new Vector3(onePath.x,onePath.y,0)));
                node = grid.NodeFromMapPoint(new Vector2(onePath.x,onePath.y));
                // all nodes should be walkable
                Assert.IsTrue(node.walkable);
            }
        }
        PathRequest.RequestPath(source, destination, OnPathFound);
        // let the A star algorithm run
        yield return new WaitForSeconds(3);
    }

    [UnityTest]
    public IEnumerator AStarInvalid()
    {
        Vector2 source = new Vector2(-7, -4);
        Vector2 destination = new Vector2(5, 2);
        // Validate that a path is NOT found
        void OnPathFound(Vector2[] newPath, bool pathSuccessful)
        {
            Assert.IsFalse(pathSuccessful);
        }
        PathRequest.RequestPath(source, destination, OnPathFound);
        // let the A star algorithm run
        yield return new WaitForSeconds(3);
    }

}

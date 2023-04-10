using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class NodeTests
{
    Node node;

    [Test]
    public void NodeTestsGridXValue()
    {
        node = new Node(true, new Vector2(1,2), 3, 4);
        Assert.AreEqual(node.gridX, 3);
    }

    [Test]
    public void NodeTestsGridYValue()
    {
        node = new Node(true, new Vector2(1,2), 3, 4);
        Assert.AreEqual(node.gridY, 4);
    }

    [Test]
    public void NodeTestsIsWalkable()
    {
        node = new Node(true, new Vector2(1,2), 3, 4);
        Assert.IsTrue(node.walkable);
    }

    [Test]
    public void NodeTestsIsNotWalkable()
    {
        node = new Node(false, new Vector2(1,2), 3, 4);
        Assert.IsFalse(node.walkable);
    }

    [Test]
    public void NodeTestsVectorValue()
    {
        node = new Node(false, new Vector2(1,2), 3, 4);
        Assert.AreEqual(node.worldposition, new Vector2(1,2));
    }

    
    [Test]
    public void NodeTestsCost()
    {
        node = new Node(false, new Vector2(1,2), 3, 4);
        node.cost = 3;
        Assert.AreEqual(node.cost, 3);
    }
    
    [Test]
    public void NodeTestsHeuristic()
    {
        node = new Node(false, new Vector2(1,2), 3, 4);
        node.heuristic = 4;
        Assert.AreEqual(node.heuristic, 4);
    }

    
    [Test]
    public void NodeTestsFValue()
    {
        node = new Node(false, new Vector2(1,2), 3, 4);
        node.cost = 3; node.heuristic = 4;
        Assert.AreEqual(node.fValue, 7);
    }
    
    [Test]
    public void NodeTestsHeapIndex()
    {
        node = new Node(false, new Vector2(1,2), 3, 4);
        node.HeapIndex = 7;
        Assert.AreEqual(node.HeapIndex, 7);
    }

    
    [Test]
    public void NodeTestsZeroCompareTo()
    {
        Node node1 = new Node(false, new Vector2(1,2), 3, 4);
        node1.cost = 3; node1.heuristic = 4; // node1 fValue is 7
        
        Node node2 = new Node(false, new Vector2(1,2), 3, 4);
        node2.cost = 4; node2.heuristic = 3; // node2 fValue is 7
        // fValues are equal, user heuristic
        int result = node1.CompareTo(node2);
        Assert.AreEqual(result, -1);
    }

    
    [Test]
    public void NodeTestsNonZeroCompareTo()
    {
        Node node1 = new Node(false, new Vector2(1,2), 3, 4);
        node1.cost = 3; node1.heuristic = 3; // node1 fValue is 6
        
        Node node2 = new Node(false, new Vector2(1,2), 3, 4);
        node2.cost = 4; node2.heuristic = 4; // node2 fValue is 8

        int result = node1.CompareTo(node2);
        Assert.AreEqual(result, 1);
    }

    [Test]
    public void NodeTestsToString()
    {
        Node node = new Node(false, new Vector2(1,2), 3, 4);
        string expectedResult = "Node: index=[3,4] position=(1.00, 2.00), walkable=False";
        Assert.AreEqual(expectedResult, node.ToString());
    }
}

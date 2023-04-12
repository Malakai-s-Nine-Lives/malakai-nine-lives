using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class HeapTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void HeapTestsSimpleAdd()
    {
        CustomHeap<HeapTestNode> heapToTest = new CustomHeap<HeapTestNode>(3);
        HeapTestNode node1 = new HeapTestNode(1,1);
        heapToTest.Add(node1);
        Assert.IsTrue(heapToTest.Contains(node1));
        Assert.AreEqual(heapToTest.Count, 1);
    }
    
    [Test]
    public void HeapTestsAddThreeNodes()
    {
        CustomHeap<HeapTestNode> heapToTest = new CustomHeap<HeapTestNode>(3);
        // create the node then add them
        HeapTestNode node1 = new HeapTestNode(1,1); heapToTest.Add(node1);
        HeapTestNode node2 = new HeapTestNode(2,2); heapToTest.Add(node2);
        HeapTestNode node3 = new HeapTestNode(3,3); heapToTest.Add(node3);
        // Check if they were all added
        Assert.IsTrue(heapToTest.Contains(node1));
        Assert.IsTrue(heapToTest.Contains(node2));
        Assert.IsTrue(heapToTest.Contains(node3));
        Assert.AreEqual(heapToTest.Count, 3);
    }
    
    [Test]
    public void HeapTestsPop()
    {
        CustomHeap<HeapTestNode> heapToTest = new CustomHeap<HeapTestNode>(3);
        // create the node then add them
        HeapTestNode node1 = new HeapTestNode(1,1); heapToTest.Add(node1);
        HeapTestNode node2 = new HeapTestNode(2,2); heapToTest.Add(node2);
        HeapTestNode node3 = new HeapTestNode(3,3); heapToTest.Add(node3);
        // Check if they were all added
        Assert.AreEqual(heapToTest.Count, 3);
        HeapTestNode nodePopped = heapToTest.Pop();
        // Check that the correct value got popped
        Assert.AreEqual(nodePopped.worth, 1);
        Assert.IsFalse(heapToTest.Contains(node1));
        Assert.AreEqual(heapToTest.Count, 2);
    }

    
    [Test]
    public void HeapTestsUpdate()
    {
        CustomHeap<HeapTestNode> heapToTest = new CustomHeap<HeapTestNode>(3);
        // create the node then add them
        HeapTestNode node1 = new HeapTestNode(1,1);
        heapToTest.Add(node1);
        HeapTestNode node2 = new HeapTestNode(1,2);
        heapToTest.UpdateItem(node2);
        // heap should update, not add
        Assert.AreEqual(heapToTest.Count, 1);
    }
}

// Class to the test the CustomHeap
public class HeapTestNode : IHeapItem<HeapTestNode>
{
    int heapIndex;
    public int worth;
    public int valueHeld;

    // Constructor for HeapTestNode
    public HeapTestNode(int _worth, int _valueHeld)
    {
        worth = _worth;
        valueHeld = _valueHeld;
    }

    public int CompareTo(HeapTestNode node)  // Comparator for node values
    {
        return node.worth - worth;
    }

    public int HeapIndex  // Get or set the HeapIndex if needed
    {
        get
        {
            return heapIndex;
        } set
        {
            heapIndex = value;
        }
    }
}
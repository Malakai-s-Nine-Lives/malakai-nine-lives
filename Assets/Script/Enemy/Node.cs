using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    // Grid values
    public bool walkable;
    public Vector2 worldposition;
    public int gridX;
    public int gridY;

    // To store the cost and heuristic values
    public int cost;
    public int heuristic;
    public Node parent;  // Will need parent for traversal
    int heapIndex;

    // Constructor for node
    public Node(bool _walkable, Vector2 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;  // Is the node walkable?
        worldposition = _worldPos;  // Where is the node?
        gridX = _gridX;  // The x point on the grid
        gridY = _gridY;  // The y point on the grid
    }

    public int fValue  // Get the f_value used to guide A*
    {
        get
        {
            return cost + heuristic;
        }
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

    public int CompareTo(Node node)  // Comparator for node values
    {
        int compare = fValue.CompareTo(node.fValue);  // Compare f-values
        if (compare == 0)  // If equal then compare the heuristic to break tie
        {
            compare = heuristic.CompareTo(node.heuristic);

        }
        return -compare;
    }
}

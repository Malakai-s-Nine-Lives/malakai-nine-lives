using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector3 worldposition;
    public int gridX;
    public int gridY;

    public int cost;
    public int heuristic;
    public Node parent;

    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldposition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int fValue
    {
        get
        {
            return cost + heuristic;
        }
    }
}

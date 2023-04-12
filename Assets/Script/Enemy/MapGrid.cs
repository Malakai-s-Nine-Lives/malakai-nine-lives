using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGrid : MonoBehaviour
{
    Node[,] grid;  // Store Grid Nodes
    public bool drawGrid;  // If we want to draw the grid
    public Vector2 gridWorldSize;  // The size of our world map
    public float nodeRadius; // The radius of each cell (Node) in the grid
    public LayerMask unwalkableMask;  // What we consider unwalkable

    float nodeDiameter;  // Diameter of each cell (Node) in the grid
    int gridSizeX, gridSizeY;  // Grid sizes

    private void Awake()  // Calculate our sizes for Nodes before anything else
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    public void Construct(Node[,] inputGrid)
    {
        grid = inputGrid;
    }

    public int MaxSize  // The size of our grid
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }


    private void OnDrawGizmos()  // Used to visualize our grid in the Scene view
    {
        Gizmos.DrawWireCube(transform.position, new Vector2(gridWorldSize.x, gridWorldSize.y));

        if (grid != null && drawGrid)
        {
            foreach (Node n in grid)  // Walkable areas are white, unwalkable are red
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(n.worldposition, Vector2.one * (nodeDiameter - 0.1f));
            }
        }
    }

    public Node NodeFromMapPoint(Vector2 worldPosition)
    {
        // Get where we are on the map in terms of a percentage
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        // Get the grid node associated with position and return it
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];

        // We use the bottom left corner of our grid to undrestand how everything else is places
        Vector2 worldBottomLeft = (Vector2)transform.position - Vector2.right * gridWorldSize.x / 2 - Vector2.up * gridWorldSize.y /2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                // Get the point for a cell in the grid
                Vector2 worldPoint = worldBottomLeft + Vector2.right * (x * nodeDiameter + nodeRadius) + Vector2.up * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask));  // Check if walkable
                grid[x, y] = new Node(walkable, worldPoint, x, y);  // Add to grid
            }
        }
    }

    public List<Node> getNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();  // List of neighbours to point

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;  // Don't add yourself as a neighbour
                }

                // The x and y values of the neigbour we are checking
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    // Add the neighbour if it is on the grid and is not yourself
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

}

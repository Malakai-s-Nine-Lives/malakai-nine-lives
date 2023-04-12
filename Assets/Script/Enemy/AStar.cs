using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/*
 * This module satisfies:
 *   - Functional requirement 2.3
 */
public class AStar : MonoBehaviour
{
    PathRequest requestManager;
    public MapGrid grid;

    void Awake()  // Get componenets before start
    {
        grid = GetComponent<MapGrid>();
        requestManager = GetComponent<PathRequest>();
    }


    public void startFindPath(Vector2 start, Vector2 end)
    {
        // To called by other classes to run A*
        StartCoroutine(AStarPathFind(start, end));
    }

    IEnumerator AStarPathFind(Vector2 start, Vector2 goal)
    {
        // Get grid values of start and goal positions
        Node startNode = grid.NodeFromMapPoint(start);
        Node goalNode = grid.NodeFromMapPoint(goal);

        Vector2[] waypoints = new Vector2[0];  // To return the path points
        bool pathSuccess = false;  // Currently we have not found a solution

        // Ensure we can access the start and end positions
        if (startNode.walkable && goalNode.walkable) {

            // Open is a heap that will store values that we can expand
            CustomHeap<Node> open = new CustomHeap<Node>(grid.MaxSize);

            // Closed will store values for nodes that we have expanded
            HashSet<Node> closed = new HashSet<Node>();
            open.Add(startNode);  // Add start node to explore

            while (open.Count > 0)  // Analyze all nodes we can
            {
                Node currentNode = open.Pop();  // Get the first value to analyze
                closed.Add(currentNode);  // Add it to closed

                if (currentNode == goalNode)  // Check if we found our goal
                {
                    pathSuccess = true;
                    break;
                }
                foreach (Node neighbour in grid.getNeighbours(currentNode))  // Check all neighbours
                {
                    if (!neighbour.walkable || closed.Contains(neighbour))  // Ensure we can access the neighbour
                    {
                        continue;
                    }

                    // Get cost of the next neighbour
                    int costNextNeighbour = currentNode.cost + getDistance(currentNode, neighbour);
                    if (costNextNeighbour < neighbour.cost || !open.Contains(neighbour))
                    {
                        // Store neighbour information
                        neighbour.cost = costNextNeighbour;
                        neighbour.heuristic = getDistance(neighbour, goalNode);
                        neighbour.parent = currentNode;

                        // Add neighbour to open if the first time visiting
                        if (!open.Contains(neighbour))
                        {
                            open.Add(neighbour);
                        } else  // Update value in open for better result
                        {
                            open.UpdateItem(neighbour);
                        }
                    }
                }
            }
        }
        yield return null;
        if (pathSuccess)
        {
            waypoints = retracePath(startNode, goalNode);  // Retrace our best path
        }
        requestManager.FinishProcessingPath(waypoints, pathSuccess);  // We will return the path
    }

    Vector2[] retracePath(Node start, Node goal)  // Retrace our A* path
    {
        List<Node> path = new List<Node>();  // To store the path
        Node currentNode = goal;  // We will work backwords
        while (currentNode != start)
        {
            path.Add(currentNode);  // Add current node
            currentNode = currentNode.parent;  // Get parent to analyze
        }

        Vector2[] waypoints = SimplifyPath(path);  // Simplify the path to waypoitns for movement
        Array.Reverse(waypoints);  // Flip array so it is sorted from start to end
        return waypoints;
    }

    Vector2[] SimplifyPath(List<Node> path)
    {
        List<Vector2> waypoints = new List<Vector2>();  // List of waypoints
        Vector2 directionOld = Vector2.zero; 

        for (int i = 1; i < path.Count; i++)
        {
            // Get a waypoint for the direction in the path
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].worldposition);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();  // Create an array of waypoints to return
    }

    int getDistance(Node nodeA, Node nodeB)  // Get Distance between nodes
    {
        int deltaX = Mathf.Abs(nodeA.gridX - nodeB.gridX);  // Change in X
        int deltaY = Mathf.Abs(nodeA.gridY - nodeB.gridY);  // Change in Y

        if (deltaX > deltaY)  // The distance formula we use
        {
            return 14 * deltaY + 10 * (deltaX - deltaY); 
        } else
        {
            return 14 * deltaX + 10 * (deltaY - deltaX);
        }
    }
}

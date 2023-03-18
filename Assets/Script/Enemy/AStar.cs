using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{

    MapGrid grid;
    public Transform malakai, enemy;

    void Awake()
    {
        grid = GetComponent<MapGrid>();
    }

    private void Update()
    {
        AStarPathFind(enemy.position, malakai.position); 
    }

    void AStarPathFind(Vector3 start, Vector3 goal)
    {
        Node startNode = grid.NodeFromMapPoint(start);
        Node goalNode = grid.NodeFromMapPoint(goal);

        List<Node> open = new List<Node>();
        HashSet<Node> closed = new HashSet<Node>();
        open.Add(startNode);

        while (open.Count > 0)
        {
            Node currentNode = open[0];
            for (int i = 1; i < open.Count; i++)
            {
                if (open[i].fValue < currentNode.fValue || open[i].fValue == currentNode.fValue && open[i].heuristic < currentNode.heuristic)
                {
                    currentNode = open[i];
                } 
            }

            open.Remove(currentNode);
            closed.Add(currentNode);

            if (currentNode == goalNode)
            {
                retracePath(startNode, goalNode);
                return;
            }
            foreach (Node neighbour in grid.getNeighbours(currentNode))
            {
                if (!neighbour.walkable || closed.Contains(neighbour))
                {
                    continue;
                }

                int costNextNeighbour = currentNode.cost + getDistance(currentNode, neighbour);
                if (costNextNeighbour < neighbour.cost || !open.Contains(neighbour))
                {
                    neighbour.cost = costNextNeighbour;
                    neighbour.heuristic = getDistance(neighbour, goalNode);
                    neighbour.parent = currentNode;

                    if (!open.Contains(neighbour))
                    {
                        open.Add(neighbour);
                    }
                }
            }
        }

    }

    void retracePath(Node start, Node goal)
    {
        List<Node> path = new List<Node>();
        Node currentNode = goal;
        while (currentNode != start)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();

        grid.path = path;
    }

    int getDistance(Node nodeA, Node nodeB)
    {
        int deltaX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int deltaY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (deltaX > deltaY)
        {
            return 14 * deltaY + 10 * (deltaX - deltaY);
        } else
        {
            return 14 * deltaX + 10 * (deltaY - deltaX);
        }
    }
}

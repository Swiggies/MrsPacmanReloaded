using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This handles pathfinding for individual ghosts
public class Pathfinding : MonoBehaviour
{
    public AStarGrid grid;

    public List<Node> path = new List<Node>();

    private void Awake()
    {
        //grid = GetComponent<Grid>();
    }

    private void Update()
    {
        //FindPath(StartPos.position, TargetPos.position);
    }

    // Finds the path between the start pos and the target pos
    // Don't completely understand it myself but I'll try my best
    public void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        if (grid == null)
            return;

        Node startNode = grid.NodeFromWorldPosition(startPos);
        Node targetNode = grid.NodeFromWorldPosition(targetPos);

        // 2 lists, keeps track of nodes that are "open" and nodes that are "closed"
        // Open nodes are the nodes that have been discovered but not been evaluated
        // Closed nodes have bee evaluated
        // Closed list is a HashSet which is similar to a list but doesn't hold any values
        // Closed nodes will not have to be checked once added to the closed list
        List<Node> OpenList = new List<Node>();
        HashSet<Node> ClosedList = new HashSet<Node>();

        OpenList.Add(startNode);

        while (OpenList.Count > 0)
        {
            // Evaluate the current set of open nodes
            Node currentNode = OpenList[0];
            for (int i = 0; i < OpenList.Count; i++)
            {
                if(OpenList[i].FCost < currentNode.FCost || OpenList[i].FCost == currentNode.FCost && OpenList[i].hCost > currentNode.hCost)
                {
                    currentNode = OpenList[i];
                }
            }
            OpenList.Remove(currentNode);
            ClosedList.Add(currentNode);

            // If the current node is the target node
            // Get the final path
            if(currentNode == targetNode)
            {
                GetFinalPath(startNode, targetNode);
                break;
            }

            // Get all the neighbor nodes next to the current node
            // Calculate their cost
            // Add them to the open list if they are not a wall
            foreach (Node neighborNode in grid.GetNeighborNodes(currentNode))
            {
                if (!neighborNode.NotWall || ClosedList.Contains(neighborNode))
                    continue;

                int moveCost = currentNode.gCost + GetManhattenDistance(currentNode, neighborNode);

                if(moveCost < neighborNode.FCost || !OpenList.Contains(neighborNode))
                {
                    neighborNode.gCost = moveCost;
                    neighborNode.hCost = GetManhattenDistance(neighborNode, targetNode);

                    neighborNode.Parent = currentNode;

                    if (!OpenList.Contains(neighborNode))
                        OpenList.Add(neighborNode);
                }
            }
        }

    }

    // Returns the final path to the current node
    void GetFinalPath(Node startNode, Node endNode)
    {
        List<Node> FinalPath = new List<Node>();
        Node currentNode = endNode;

        while(currentNode != startNode)
        {
            FinalPath.Add(currentNode);
            currentNode = currentNode.Parent;
        }

        FinalPath.Reverse();

        path = FinalPath;
    }

    // Returns the manhatten distance between 2 nodes
    int GetManhattenDistance(Node nodeA, Node nodeB)
    {
        int ix = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int iy = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        return ix + iy;
    }
}

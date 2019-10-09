using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Grid grid;

    public List<Node> path = new List<Node>();

    private void Awake()
    {
        //grid = GetComponent<Grid>();
    }

    private void Update()
    {
        //FindPath(StartPos.position, TargetPos.position);
    }

    public void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = grid.NodeFromWorldPosition(startPos);
        Node targetNode = grid.NodeFromWorldPosition(targetPos);

        List<Node> OpenList = new List<Node>();
        HashSet<Node> ClosedList = new HashSet<Node>();

        OpenList.Add(startNode);
        //Debug.Log($"{startNode.gridX}:{startNode.gridY} / {targetNode.gridX}:{targetNode.gridY}");

        while (OpenList.Count > 0)
        {
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

            if(currentNode == targetNode)
            {
                GetFinalPath(startNode, targetNode);
                break;
            }

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

    int GetManhattenDistance(Node nodeA, Node nodeB)
    {
        int ix = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int iy = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        return ix + iy;
    }
}

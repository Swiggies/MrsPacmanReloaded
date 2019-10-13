using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public static Grid Instance;

    public Transform StartPos;
    public LayerMask WallMask;
    public Vector2 GridWorldSize;
    public float NodeRadius;
    public float Distance;

    Node[,] grid;
    //public List<Node> FinalPath;

    float nodeDiamater;
    int gridSizeX, gridSizeY;

    void Awake()
    {
        Instance = this;
        nodeDiamater = NodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(GridWorldSize.x / nodeDiamater);
        gridSizeY = Mathf.RoundToInt(GridWorldSize.y / nodeDiamater);
    }

    public void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];  
        Vector3 bottomLeft = transform.position - (Vector3.right * GridWorldSize.x / 2) - (Vector3.up * GridWorldSize.y / 2);
        Debug.Log(bottomLeft);
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiamater + NodeRadius) + Vector3.up * (y * nodeDiamater + NodeRadius);
                bool NotWall = true;

                if (Physics2D.OverlapCircle(worldPoint, NodeRadius - 0.1f, WallMask))
                    NotWall = false;

                grid[x, y] = new Node(NotWall, worldPoint, x, y);
            }
        }
        Debug.Log("Grid Created");
    }

    public Node NodeFromWorldPosition(Vector3 worldPos)
    {
        float xPoint = (worldPos.x / GridWorldSize.x);
        float yPoint = (worldPos.y / GridWorldSize.y);

        xPoint = Mathf.Clamp01(xPoint);
        yPoint = Mathf.Clamp01(yPoint);

        int x = Mathf.RoundToInt((gridSizeX) * xPoint);
        int y = Mathf.RoundToInt((gridSizeY) * yPoint);

        if (x == gridSizeX)
            x = gridSizeX - 1;

        if (y == gridSizeY)
            y = gridSizeY - 1;

        return grid[x, y];
    }

    public List<Node> GetNeighborNodes(Node node)
    {
        List<Node> neighboringNodes = new List<Node>();

        int xCheck;
        int yCheck;

        xCheck = node.gridX + 1;
        yCheck = node.gridY;
        if(xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }

        xCheck = node.gridX - 1;
        yCheck = node.gridY;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }

        xCheck = node.gridX;
        yCheck = node.gridY + 1;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }

        xCheck = node.gridX;
        yCheck = node.gridY - 1;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }
        return neighboringNodes;
    }

    public void SetPointToWall(Vector3 pos)
    {
        grid[(int)pos.x, (int)pos.y].NotWall = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(GridWorldSize.x, GridWorldSize.y, 1));

        if(grid != null)
        {
            foreach (Node node in grid)
            {
                if (node.NotWall)
                    Gizmos.color = Color.white;
                else
                    Gizmos.color = Color.yellow;

                Gizmos.DrawCube(node.position, Vector3.one * (nodeDiamater - Distance));
            }
        }
    }
}

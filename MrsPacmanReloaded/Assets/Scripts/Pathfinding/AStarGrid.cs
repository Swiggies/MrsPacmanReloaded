using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this is used for the A* pathfinding algorithm
public class AStarGrid : MonoBehaviour
{
    public static AStarGrid Instance;

    public Transform StartPos;
    public LayerMask WallMask;
    public Vector2 GridWorldSize;
    public float NodeRadius;
    public float Distance;

    Node[,] grid;
    //public List<Node> FinalPath;

    float nodeDiamater;
    int gridSizeX, gridSizeY;

    // set up the singleton and grid to be used for the generation
    void Awake()
    {
        Instance = this;
        nodeDiamater = NodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(GridWorldSize.x / nodeDiamater);
        gridSizeY = Mathf.RoundToInt(GridWorldSize.y / nodeDiamater);
    }

    // create the grid
    public void CreateGrid()
    {
        // Setup the grid size
        grid = new Node[gridSizeX, gridSizeY];  
        Vector3 bottomLeft = transform.position - (Vector3.right * GridWorldSize.x / 2) - (Vector3.up * GridWorldSize.y / 2);
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                // Get the world point of the grid and add a node to that position
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiamater + NodeRadius) + Vector3.up * (y * nodeDiamater + NodeRadius);
                bool NotWall = true;

                // Check if there is any collisions around the node
                if (Physics2D.OverlapCircle(worldPoint, NodeRadius - 0.1f, WallMask))
                    NotWall = false;

                // Add it to the 2D array
                grid[x, y] = new Node(NotWall, worldPoint, x, y);
            }
        }
        Debug.Log("Grid Created");
    }

    // Converts a node from world position to 2D array position
    // These are 1:1
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

    // Used by the Pathfinding class to find neighbouring nodes to construct a valid path
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

    // Sets a point on the grid to a wall
    public void SetPointToWall(Vector3 pos)
    {
        grid[(int)pos.x, (int)pos.y].NotWall = false;
    }

    // Used to draw gizmos of the grid
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

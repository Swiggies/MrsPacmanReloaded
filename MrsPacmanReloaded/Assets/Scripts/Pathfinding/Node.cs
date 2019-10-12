using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node {
    public int gridX;
    public int gridY;

    public bool NotWall;
    public Vector3 position;

    public Node Parent;

    public int gCost;
    public int hCost;

    public int FCost { get { return gCost + hCost;  } }

    public Node (bool isWall, Vector3 pos, int x, int y)
    {
        NotWall = isWall;
        position = pos;
        gridX = x;
        gridY = y;
    }
    
    public Node (bool isWall)
    {
        NotWall = isWall;
    }
}

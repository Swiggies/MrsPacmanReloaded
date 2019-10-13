using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkGhost : GhostAI
{
    public Node[] nodes = new Node[4];
    private int currentlySelectedNode = 0;

    // Start is called before the first frame update
    // When the Pink Ghost is spawned, it gets the 4 corners of the map
    public override void Start()
    {
        base.Start();
        nodes[0] = GetFirstEmptyNodeInArea(new Vector3(AStarGrid.Instance.GridWorldSize.x, AStarGrid.Instance.GridWorldSize.y, 0), 3);
        nodes[1] = GetFirstEmptyNodeInArea(new Vector3(AStarGrid.Instance.GridWorldSize.x, 0, 0), 3);
        nodes[2] = GetFirstEmptyNodeInArea(new Vector3(0, 0, 0), 3);
        nodes[3] = GetFirstEmptyNodeInArea(new Vector3(0, AStarGrid.Instance.GridWorldSize.y, 0), 3);
        Seeker.targetPos = nodes[currentlySelectedNode].position;

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (!GameManager.GameStarted)
            return;

        if (IsHaywiring || !IsAlive)
            return;

        // When the ghost reaches the end of its path, go to the next corner in the list
        if(Pathfinding.path.Count == 0 || Pathfinding.path[Pathfinding.path.Count-1] != nodes[currentlySelectedNode])
        {
            Seeker.targetPos = nodes[currentlySelectedNode].position;
        }

        // When the ghost reaches the end of its path, go to the next corner in the list
        if (Vector3.Distance(transform.position, nodes[currentlySelectedNode].position) <= 1)
        {
            currentlySelectedNode++;
            if (currentlySelectedNode > 3)
                currentlySelectedNode = 0;
            Seeker.targetPos = nodes[currentlySelectedNode].position;
        }
        if(Pathfinding.path.Count > 0)
            Tweener.Instance.AddTween(transform, transform.position, Pathfinding.path[0].position, Speed);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkGhost : GhostAI
{
    public Node[] nodes = new Node[4];
    private int currentlySelectedNode = 0;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        nodes[0] = GetFirstEmptyNodeInArea(new Vector3(Grid.instance.GridWorldSize.x, Grid.instance.GridWorldSize.y, 0), 3);
        nodes[1] = GetFirstEmptyNodeInArea(new Vector3(Grid.instance.GridWorldSize.x, 0, 0), 3);
        nodes[2] = GetFirstEmptyNodeInArea(new Vector3(0, 0, 0), 3);
        nodes[3] = GetFirstEmptyNodeInArea(new Vector3(0, Grid.instance.GridWorldSize.y, 0), 3);
        seeker.targetPos = nodes[currentlySelectedNode].position;

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (haywire || !isAlive)
            return;

        if(pathfinding.path.Count == 0 || pathfinding.path[pathfinding.path.Count-1] != nodes[currentlySelectedNode])
        {
            seeker.targetPos = nodes[currentlySelectedNode].position;
        }

        if (Vector3.Distance(transform.position, nodes[currentlySelectedNode].position) <= 1)
        {
            currentlySelectedNode++;
            if (currentlySelectedNode > 3)
                currentlySelectedNode = 0;
            seeker.targetPos = nodes[currentlySelectedNode].position;
        }
        if(pathfinding.path.Count > 0)
            Tweener._Instance.AddTween(transform, transform.position, pathfinding.path[0].position, speed);
    }
}

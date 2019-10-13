using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenGhost : GhostAI
{

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        Seeker.targetPos = GetRandomNodeInArea(Vector3.zero, (int)AStarGrid.Instance.GridWorldSize.x, (int)AStarGrid.Instance.GridWorldSize.y).position;
    }

    // Update is called once per frame
    // Green ghost is supposed to move randomly at every junction
    // Due to my grids and levels being procedural I cannot define where a junction is easily
    // I instead opted to  make the green ghost move randomly and move randomly every time it hits a wall infront of it
    // These tend to be junctions anything due to the labyrynth nature of PacMan
    public override void Update()
    {
        base.Update();
        if (!GameManager.GameStarted)
            return;

        if (IsHaywiring || !IsAlive)
            return;

        // Get the direction that the ghost is moving in
        var newDir = Vector2.zero;
        if (Pathfinding.path.Count > 0)
            newDir = (Pathfinding.path[0].position - transform.position).normalized;

        // Raycast in the direction the ghost is moving in
        // If it hits anything, find a new path to go in
        RaycastHit2D newHit = Physics2D.Raycast(transform.position, newDir, 1.5f);
        if (newHit || newDir == Vector2.zero)
        {
            Seeker.targetPos = GetRandomNodeInArea(Vector3.zero, (int)AStarGrid.Instance.GridWorldSize.x, (int)AStarGrid.Instance.GridWorldSize.y).position;
        }

        if (Pathfinding.path.Count > 0)
            Tweener.Instance.AddTween(transform, transform.position, Pathfinding.path[0].position, Speed);
    }
}

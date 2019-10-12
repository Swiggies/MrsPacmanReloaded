using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenGhost : GhostAI
{

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        seeker.targetPos = GetRandomNodeInArea(Vector3.zero, (int)Grid.Instance.GridWorldSize.x, (int)Grid.Instance.GridWorldSize.y).position;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (!GameManager.GameStarted)
            return;

        if (haywire || !isAlive)
            return;

        var newDir = Vector2.zero;
        if (pathfinding.path.Count > 0)
            newDir = (pathfinding.path[0].position - transform.position).normalized;

        RaycastHit2D newHit = Physics2D.Raycast(transform.position, newDir, 1.5f);
        if (newHit || newDir == Vector2.zero)
        {
            seeker.targetPos = GetRandomNodeInArea(Vector3.zero, (int)Grid.Instance.GridWorldSize.x, (int)Grid.Instance.GridWorldSize.y).position;
        }

        if (pathfinding.path.Count > 0)
            Tweener.Instance.AddTween(transform, transform.position, pathfinding.path[0].position, speed);
    }
}

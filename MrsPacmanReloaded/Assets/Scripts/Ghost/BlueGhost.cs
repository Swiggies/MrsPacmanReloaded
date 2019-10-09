using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueGhost : GhostAI
{
    private float cooldown;
    private int distance = 5;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (haywire || !isAlive)
            return;

        if(cooldown >= 0.1f)
        {
            distance = 5;
        }
        else
            cooldown += Time.deltaTime;


        if (Vector2.Distance(seeker.targetPos, player.position) <= distance)
        {
            GetNewPosition();
        }

        if (Vector2.Distance(transform.position, player.position) <= distance)
        {
            GetNewPosition();
        }
        if (!Grid.instance.NodeFromWorldPosition(seeker.targetPos).NotWall)
            GetNewPosition();

        if (pathfinding.path.Count > 0)
            Tweener._Instance.AddTween(transform, transform.position, pathfinding.path[0].position, speed);
        else
            GetNewPosition();
    }

    private void GetNewPosition()
    {
        cooldown = 0;
        distance = 0;
        seeker.targetPos = GetRandomNodeInArea(Vector3.zero, (int)Grid.instance.GridWorldSize.x, (int)Grid.instance.GridWorldSize.y).position;
    }
}

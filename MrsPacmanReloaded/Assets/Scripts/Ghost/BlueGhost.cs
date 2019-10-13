using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueGhost : GhostAI
{
    private float cooldown;
    private int distance = 5;
    Vector2 currentDir;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (!GameManager.GameStarted)
            return;

        if (haywire || !isAlive)
            return;

        if(cooldown >= 0.1f)
        {
            distance = 5;
        }
        else
            cooldown += Time.deltaTime;

        if (pathfinding.path.Count > 0)
        {
            currentDir = (pathfinding.path[0].position - transform.position).normalized;
            Debug.DrawRay(transform.position, currentDir);
            RaycastHit2D newHit = Physics2D.Raycast(transform.position, currentDir);
            if (newHit != false)
            {
                if (newHit.collider.CompareTag("Player"))
                {
                    Debug.Log("Blue ghost seen player");
                    GetNewPosition();
                }
            }
        }

        if (Vector2.Distance(seeker.targetPos, player.position) <= distance)
        {
            GetNewPosition();
        }

        if (!Grid.Instance.NodeFromWorldPosition(seeker.targetPos).NotWall)
            GetNewPosition();

        if (pathfinding.path.Count > 0)
            Tweener.Instance.AddTween(transform, transform.position, pathfinding.path[0].position, speed);
        else
            GetNewPosition();
    }

    private void GetNewPosition()
    {
        cooldown = 0;
        distance = 0;
        seeker.targetPos = GetRandomNodeInArea(Vector3.zero, (int)Grid.Instance.GridWorldSize.x, (int)Grid.Instance.GridWorldSize.y).position;
    }
}

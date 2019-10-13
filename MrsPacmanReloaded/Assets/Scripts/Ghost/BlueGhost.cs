using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Child of GhostAI class
public class BlueGhost : GhostAI
{

    // private members
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
        // If the game has not started, the ghost will not do anything
        base.Update();
        if (!GameManager.GameStarted)
            return;

        // if the ghost is dead or haywiring, ignore this
        if (IsHaywiring || !IsAlive)
            return;

        // cooldown for if the blue ghost has seen the player
        // this is used so that the ghost does not keep trying to find paths
        if(cooldown >= 0.1f)
        {
            distance = 5;
        }
        else
            cooldown += Time.deltaTime;

        // if the ghost has a path, do the following
        if (Pathfinding.path.Count > 0)
        {
            // get the current direction of the ghosts movement
            // raycast in the direction and see if the player is there
            // if the raycast hits the player, try to go in a different direction
            currentDir = (Pathfinding.path[0].position - transform.position).normalized;
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

        // if the player is near where the ghost wants to go
        // find a new position
        if (Vector2.Distance(Seeker.targetPos, player.position) <= distance)
        {
            GetNewPosition();
        }

        // if the ghost happens to try to want to go into a wall
        // don't do that
        if (!AStarGrid.Instance.NodeFromWorldPosition(Seeker.targetPos).NotWall)
            GetNewPosition();

        // if the ghost has a path, tween through it
        // otherwise find a new position to go to
        if (Pathfinding.path.Count > 0)
            Tweener.Instance.AddTween(transform, transform.position, Pathfinding.path[0].position, Speed);
        else
            GetNewPosition();
    }

    // find a new position to move to
    private void GetNewPosition()
    {
        cooldown = 0;
        distance = 0;
        Seeker.targetPos = GetRandomNodeInArea(Vector3.zero, (int)AStarGrid.Instance.GridWorldSize.x, (int)AStarGrid.Instance.GridWorldSize.y).position;
    }
}

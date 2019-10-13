using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedGhost : GhostAI
{

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    // Red ghost is the most simple. Gets the position of the player and tries to move towards it using the A* algorithm
    public override void Update()
    {
        base.Update();
        if (!GameManager.GameStarted)
            return;

        if (IsHaywiring || !IsAlive)
            return;

        Seeker.targetPos = player.transform.position;
        if (Pathfinding.path.Count > 0)
            Tweener.Instance.AddTween(transform, transform.position, Pathfinding.path[0].position, Speed);
    }
}

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
    public override void Update()
    {
        base.Update();
        if (haywire || !isAlive)
            return;

        seeker.targetPos = player.transform.position;
        if (pathfinding.path.Count > 0)
            Tweener._Instance.AddTween(transform, transform.position, pathfinding.path[0].position, speed);
    }
}

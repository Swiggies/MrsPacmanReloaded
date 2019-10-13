using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is an intermediate class between the GhostAI and the Pathfinding
public class Seeker : MonoBehaviour
{
    public Vector3 targetPos;

    Pathfinding pathfinding;

    // Start is called before the first frame update
    void Start()
    {
        pathfinding = GetComponent<Pathfinding>();
    }

    // Update is called once per frame
    // Every frame, if our target position is not null
    // Find the path to the target position
    void Update()
    {
        if (targetPos == null)
            return;

        pathfinding.FindPath(transform.position, targetPos);
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        if (pathfinding.path.Count > 0)
        {
            foreach (Node n in pathfinding.path)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(n.position, 0.5f);
            }
        }
    }
}

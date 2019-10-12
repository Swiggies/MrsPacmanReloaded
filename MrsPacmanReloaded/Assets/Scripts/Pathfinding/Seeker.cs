using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeker : MonoBehaviour
{
    public Vector3 targetPos;

    Pathfinding pathfinding;

    bool nextMove = true;

    // Start is called before the first frame update
    void Start()
    {
        pathfinding = GetComponent<Pathfinding>();
    }

    // Update is called once per frame
    void Update()
    {
        if (targetPos == null)
            return;

        pathfinding.FindPath(transform.position, targetPos);
        //if (pathfinding.path.Count > 0 && nextMove)
        //    StartCoroutine(MoveTowards(pathfinding.path[0].position, .25f));
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

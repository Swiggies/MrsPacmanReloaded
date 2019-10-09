using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAI : MonoBehaviour
{
    private Vector3 startPos;
    public Color defaultColor;
    public Color haywireColor;
    public Seeker seeker;
    public Pathfinding pathfinding;
    public static float speed = 0.3f;
    public static Transform player;

    public bool haywire = false;
    private float haywireTimer = 10;

    public bool isAlive = true;

    public delegate void DeathHandler();
    public static event DeathHandler OnDeath;

    // Start is called before the first frame update
    public virtual void Start()
    {
        startPos = transform.position;
        pathfinding = GetComponent<Pathfinding>();
        pathfinding.grid = Grid.instance;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        defaultColor = GetComponent<SpriteRenderer>().color;

        Haywire.OnHaywire += OnHaywire;
    }

    private void OnHaywire()
    {
        Debug.Log("OnHaywire");
        haywire = true;
        haywireTimer = 10;
        GetComponent<SpriteRenderer>().color = haywireColor;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (!isAlive)
        {
            Die();
        }
        if (haywire)
        {
            haywireTimer -= Time.deltaTime;
            if(haywireTimer <= 0)
            {
                StopHaywire();
            }
            seeker.targetPos = GetRandomNodeInArea(Vector3.zero, (int)Grid.instance.GridWorldSize.x, (int)Grid.instance.GridWorldSize.y).position;
            if (pathfinding.path.Count > 0)
                Tweener._Instance.AddTween(transform, transform.position, pathfinding.path[0].position, 0.25f);
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(!haywire && col.CompareTag("Player"))
        {
            // kill player
            Debug.Log($"Player is dead");
        }
        else if (haywire && col.CompareTag("Player"))
        {
            // die
            Debug.Log($"{gameObject.name} is dead");
            if (isAlive)
            {
                GetComponent<AudioSource>().Play();
                transform.localScale = Vector3.one * 0.5f;
                isAlive = false;
                haywire = false;
            }
        }
    }

    private void StopHaywire()
    {
        haywire = false;
        haywireTimer = 10;
        GetComponent<SpriteRenderer>().color = defaultColor;
    }

    private void Die()
    {
        seeker.targetPos = startPos;
        if (pathfinding.path.Count > 0)
            Tweener._Instance.AddTween(transform, transform.position, pathfinding.path[0].position, 0.05f);

        if (Vector2.Distance(transform.position, startPos) < 0.05f)
        {
            transform.localScale = Vector3.one;
            isAlive = true;
            StopHaywire();
        }
    }

    public Node GetFirstEmptyNodeInArea(Vector3 pos, int size)
    {
        for (int x = -size; x < size; x++)
        {
            for (int y = -size; y < size; y++)
            {
                var node = Grid.instance.NodeFromWorldPosition(pos + new Vector3(x, y));
                if (node.NotWall)
                    return node;
                else
                    continue;
            }
        }
        return null;
    }

    public Node GetRandomNodeInArea(Vector3 pos, int xSize, int ySize)
    {
        Vector3 randomPos = pos + new Vector3(Random.Range(0, xSize), Random.Range(0, ySize));
        Node node;
            node = Grid.instance.NodeFromWorldPosition(randomPos);
        return node;
    }
}

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
        pathfinding.grid = Grid.Instance;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        defaultColor = GetComponent<SpriteRenderer>().color;

        Haywire.OnHaywire += OnHaywire;
        GameManager.OnGameRestart += OnGameRestart;
    }

    private void OnGameRestart(GameManager.GameStates state)
    {
        Tweener.Instance.CancelTween(transform);
        StopHaywire();
        pathfinding.path.Clear();
        transform.position = startPos;
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
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        if (!isAlive)
        {
            Die();
        }
        if (haywire)
        {
            haywireTimer -= Time.deltaTime;
            if (haywireTimer <= 0)
            {
                StopHaywire();
            }
            seeker.targetPos = GetRandomNodeInArea(Vector3.zero, (int)Grid.Instance.GridWorldSize.x, (int)Grid.Instance.GridWorldSize.y).position;
            if (pathfinding.path.Count > 0)
                Tweener.Instance.AddTween(transform, transform.position, pathfinding.path[0].position, 0.25f);
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(!haywire && col.CompareTag("Player"))
        {
            // kill player
            col.GetComponent<PlayerController>().KillPlayer();
            Debug.Log($"Player is dead");
        }
        else if (haywire && col.CompareTag("Player"))
        {
            // die
            Debug.Log($"{gameObject.name} is dead");
            if (isAlive)
            {
                SetDeath();
            }
        }
        else if (col.CompareTag("SuperPlayer"))
        {
            // die
            Debug.Log($"{gameObject.name} is dead");
            if (isAlive)
            {
                SetDeath();
            }
        }
    }

    private void SetDeath()
    {
        Tweener.Instance.CancelTween(transform);
        GetComponent<SpriteRenderer>().color = haywireColor;

        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<AudioSource>().Play();
        transform.localScale = Vector3.one * 0.5f;
        isAlive = false;
        haywire = false;
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
            Tweener.Instance.AddTween(transform, transform.position, pathfinding.path[0].position, 0.05f);

        if (Vector2.Distance(transform.position, startPos) < 0.05f)
        {
            transform.localScale = Vector3.one;
            isAlive = true;
            GetComponent<CircleCollider2D>().enabled = true;
            StopHaywire();
        }
    }

    private void OnDisable()
    {
        Haywire.OnHaywire -= OnHaywire;
        GameManager.OnGameRestart -= OnGameRestart;
    }

    public Node GetFirstEmptyNodeInArea(Vector3 pos, int size)
    {
        for (int x = -size; x < size; x++)
        {
            for (int y = -size; y < size; y++)
            {
                var node = Grid.Instance.NodeFromWorldPosition(pos + new Vector3(x, y));
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
            node = Grid.Instance.NodeFromWorldPosition(randomPos);
        return node;
    }
}

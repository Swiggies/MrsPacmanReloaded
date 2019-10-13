using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAI : MonoBehaviour
{
    // Static members
    public static float Speed = 0.3f;
    public static Transform player;

    // Public members
    public Color DefaultColor;
    public Color HaywireColor;
    public Seeker Seeker;
    public Pathfinding Pathfinding;
    public bool IsHaywiring = false;
    public bool IsAlive = true;

    // private members
    private Vector3 startPos;
    private float haywireTimer = 10;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private CircleCollider2D circleCollider;

    // event and event hanlder
    public delegate void DeathHandler();
    public static event DeathHandler OnDeath;

    // Start is called before the first frame update
    // Setting up the ghost
    public virtual void Start()
    {
        startPos = transform.position;
        Pathfinding = GetComponent<Pathfinding>();
        Pathfinding.grid = AStarGrid.Instance;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        DefaultColor = GetComponent<SpriteRenderer>().color;

        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        circleCollider = GetComponent<CircleCollider2D>();

        Haywire.OnHaywire += OnHaywire;
        GameManager.OnGameRestart += OnGameRestart;
    }

    // Called when the game is restart
    // Cleans up the ghost and resets it to its spawn position
    private void OnGameRestart(GameManager.GameStates state)
    {
        Tweener.Instance.CancelTween(transform);
        StopHaywire();
        Pathfinding.path.Clear();
        transform.position = startPos;
    }

    // When the haywire powerup is used this is called
    // Sets the color of the ghost and the timer
    private void OnHaywire()
    {
        Debug.Log("OnHaywire");
        IsHaywiring = true;
        haywireTimer = 10;
        spriteRenderer.color = HaywireColor;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        // If the player can't be found, find it
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        // If the ghost is not alive this is called
        if (!IsAlive)
        {
            Die();
        }

        // If the ghost is haywiring this is called
        // The ghost will find random spots on the level to go towards
        // Making their movements erratic
        if (IsHaywiring)
        {
            haywireTimer -= Time.deltaTime;
            if (haywireTimer <= 0)
            {
                StopHaywire();
            }
            Seeker.targetPos = GetRandomNodeInArea(Vector3.zero, (int)AStarGrid.Instance.GridWorldSize.x, (int)AStarGrid.Instance.GridWorldSize.y).position;
            if (Pathfinding.path.Count > 0)
                Tweener.Instance.AddTween(transform, transform.position, Pathfinding.path[0].position, 0.25f);
            return;
        }

        // The seperate ghost classes continue this
    }


    // Whenever the ghost "collides" with something
    private void OnTriggerEnter2D(Collider2D col)
    {
        // If we are not haywiring and the player is touched
        // Kill the player
        if(!IsHaywiring && col.CompareTag("Player"))
        {
            // kill player
            col.GetComponent<PlayerController>().KillPlayer();
            Debug.Log($"Player is dead");
        }
        // If we are haywiring and we touch the player
        // We die
        else if (IsHaywiring && col.CompareTag("Player"))
        {
            // die
            Debug.Log($"{gameObject.name} is dead");
            if (IsAlive)
            {
                SetDeath();
            }
        }

        // If the player is "super" (using boost) we die
        else if (col.CompareTag("SuperPlayer"))
        {
            // die
            Debug.Log($"{gameObject.name} is dead");
            if (IsAlive)
            {
                SetDeath();
            }
        }
    }

    // This is used to easily set the death state of the ghost
    private void SetDeath()
    {
        Tweener.Instance.CancelTween(transform);
        spriteRenderer.color = HaywireColor;
        circleCollider.enabled = false;
        audioSource.Play();
        transform.localScale = Vector3.one * 0.5f;
        IsAlive = false;
        IsHaywiring = false;
    }

    // Used to stop the ghost haywiring
    // When the timer is up or the ghost reaches their spawn
    private void StopHaywire()
    {
        IsHaywiring = false;
        haywireTimer = 10;
        spriteRenderer.color = DefaultColor;
    }

    // This is called in Update() when the ghost is dead
    // Is finished when the player is 0.05 units from their spawn
    private void Die()
    {
        Seeker.targetPos = startPos;
        if (Pathfinding.path.Count > 0)
            Tweener.Instance.AddTween(transform, transform.position, Pathfinding.path[0].position, 0.05f);

        if (Vector2.Distance(transform.position, startPos) < 0.05f)
        {
            transform.localScale = Vector3.one;
            IsAlive = true;
            circleCollider.enabled = true;
            StopHaywire();
        }
    }

    // Called when the ghost is destroyed, to set up for a new level
    private void OnDisable()
    {
        Haywire.OnHaywire -= OnHaywire;
        GameManager.OnGameRestart -= OnGameRestart;
    }

    // Used to pathfind
    // Return the first empty node within a given area
    public Node GetFirstEmptyNodeInArea(Vector3 pos, int size)
    {
        for (int x = -size; x < size; x++)
        {
            for (int y = -size; y < size; y++)
            {
                var node = AStarGrid.Instance.NodeFromWorldPosition(pos + new Vector3(x, y));
                if (node.NotWall)
                    return node;
                else
                    continue;
            }
        }
        return null;
    }

    // Returns a random node within a given area
    // Used when haywiring mostly
    public Node GetRandomNodeInArea(Vector3 pos, int xSize, int ySize)
    {
        Vector3 randomPos = pos + new Vector3(Random.Range(0, xSize), Random.Range(0, ySize));
        Node node;
            node = AStarGrid.Instance.NodeFromWorldPosition(randomPos);
        return node;
    }
}

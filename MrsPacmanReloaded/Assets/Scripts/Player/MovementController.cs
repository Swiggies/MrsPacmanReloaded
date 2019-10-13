using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles the players movements
public class MovementController : MonoBehaviour
{
    // Default speed, used to reset if changed
    private const float DEFAULT_SPEED = 0.25f;

    // Public members
    public static float DefaultSpeed => DEFAULT_SPEED;
    public float Speed = 0.25f;
    public Vector2 CurrentDir { get; private set; }
    public bool LockedDir = false;

    // Private members
    [SerializeField] private int targetFPS = 60;
    private PlayerController playerController;


    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = targetFPS;
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        // If the game has not started, the player cannot use any input or move
        if (!GameManager.GameStarted)
            return;

        // IF the player is dead, the player cannot use any input or move
        if (playerController.IsDead)
        {
            Tweener.Instance.CancelTween(transform);
            return;
        }

        // If the direction is locked, the player cannot use any input but still move
        if (!LockedDir)
        {

            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                float inputX = Input.GetAxisRaw("Horizontal");
                float inputY = Input.GetAxisRaw("Vertical");

                // If the player has pressed 2 directional buttons at once
                // The y axis is set to 0 so the player can only move left or right if 2 directions are pressed
                if (Mathf.Abs(inputY) > 0 && Mathf.Abs(inputX) > 0)
                    inputY = 0;

                Vector2 newDir = new Vector2(inputX, inputY);

                if (newDir != CurrentDir)
                    CurrentDir = new Vector2(inputX, inputY);
            }
        }

        // Checks collisions and sets the currect direction of the player accordingly
        if (CheckCollision(new Vector2(CurrentDir.x, 0)))
            CurrentDir = new Vector2(0, CurrentDir.y);
        if (CheckCollision(new Vector2(0, CurrentDir.y)))
            CurrentDir = new Vector2(CurrentDir.x, 0);

        // Makes the player tween and rotates the sprite
        if (Tweener.Instance.AddTween(transform, transform.position, transform.position + new Vector3(CurrentDir.x, CurrentDir.y, 0), Speed))
        {
            transform.right = CurrentDir;
        }

        // If the player goes too far to the left off screen
        // Set the player to the opposite side
        if (transform.position.x <= -0.5f)
        {
            transform.position = new Vector3(AStarGrid.Instance.GridWorldSize.x - 1, transform.position.y);
            Tweener.Instance.CancelTween(transform);
            Tweener.Instance.AddTween(transform, transform.position, transform.position + new Vector3(CurrentDir.x, CurrentDir.y, 0), Speed);
        }

        // If the player goes too far to the right off screen
        // Set the player to the opposite side
        if (transform.position.x >= AStarGrid.Instance.GridWorldSize.x - 0.5f)
        {
            transform.position = new Vector3(0, transform.position.y);
            Tweener.Instance.CancelTween(transform);
            Tweener.Instance.AddTween(transform, transform.position, transform.position + new Vector3(CurrentDir.x, CurrentDir.y, 0), Speed);
        }
    }

    // Checks for collisions in front of the player
    private bool CheckCollision(Vector2 dir)
    {

        RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(0.9f, 0.9f), 0, dir, 0.1f, LayerMask.GetMask("Wall"));
        if(hit)
            return true;

        return false;
    }
}

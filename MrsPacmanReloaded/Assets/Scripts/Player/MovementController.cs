using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    private const float DEFAULT_SPEED = 0.25f;

    public static float DefaultSpeed => DEFAULT_SPEED;
    public float Speed = 0.25f;
    public Vector2 CurrentDir { get; private set; }
    public bool LockedDir = false;

    [SerializeField] private int targetFPS = 60;
    private PlayerController playerController;


    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = targetFPS;
        playerController = GetComponent<PlayerController>();
        //StartCoroutine("Movement");
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.GameStarted)
            return;

        if (playerController.IsDead)
        {
            Tweener.Instance.CancelTween(transform);
            return;
        }

        if (!LockedDir)
        {

            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                float inputX = Input.GetAxisRaw("Horizontal");
                float inputY = Input.GetAxisRaw("Vertical");

                if (Mathf.Abs(inputY) > 0 && Mathf.Abs(inputX) > 0)
                    inputY = 0;

                Vector2 newDir = new Vector2(inputX, inputY);

                if (newDir != CurrentDir)
                    CurrentDir = new Vector2(inputX, inputY);
            }
        }

        if (CheckCollision(new Vector2(CurrentDir.x, 0)))
            CurrentDir = new Vector2(0, CurrentDir.y);
        if (CheckCollision(new Vector2(0, CurrentDir.y)))
            CurrentDir = new Vector2(CurrentDir.x, 0);

        if (Tweener.Instance.AddTween(transform, transform.position, transform.position + new Vector3(CurrentDir.x, CurrentDir.y, 0), Speed))
        {
            transform.right = CurrentDir;
        }

        if (transform.position.x <= -0.5f)
        {
            transform.position = new Vector3(Grid.Instance.GridWorldSize.x - 1, transform.position.y);
            Tweener.Instance.CancelTween(transform);
            Tweener.Instance.AddTween(transform, transform.position, transform.position + new Vector3(CurrentDir.x, CurrentDir.y, 0), Speed);
        }

        if (transform.position.x >= Grid.Instance.GridWorldSize.x - 0.5f)
        {
            transform.position = new Vector3(0, transform.position.y);
            Tweener.Instance.CancelTween(transform);
            Tweener.Instance.AddTween(transform, transform.position, transform.position + new Vector3(CurrentDir.x, CurrentDir.y, 0), Speed);
        }
    }

    private bool CheckCollision(Vector2 dir)
    {

        RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(0.9f, 0.9f), 0, dir, 0.1f);
        if(hit)
            return true;

        return false;
    }
}

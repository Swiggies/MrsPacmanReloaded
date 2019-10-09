using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private int targetFPS = 60;
    [SerializeField] private float speed = 0.25f;

    private Vector2 currentDir;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = targetFPS;
        //StartCoroutine("Movement");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0){
            float inputX = Input.GetAxisRaw("Horizontal");
            float inputY = Input.GetAxisRaw("Vertical");

            Vector2 newDir = new Vector2(inputX, inputY);

            if (newDir != currentDir)
                currentDir = new Vector2(inputX, inputY);
        }

        if (CheckCollision(currentDir))
            currentDir.x = 0;
        if (CheckCollision(new Vector2(currentDir.x, 0)))
            currentDir.x = 0;
        if (CheckCollision(new Vector2(0, currentDir.y)))
            currentDir.y = 0;

        if(Tweener._Instance.AddTween(transform, transform.position, transform.position + new Vector3(currentDir.x, currentDir.y, 0), speed))
        {
            transform.right = currentDir;
        }

        if (transform.position.x <= -0.5f)
        {
            transform.position = new Vector3(Grid.instance.GridWorldSize.x - 1, transform.position.y);
            Tweener._Instance.CancelTween(transform);
            Tweener._Instance.AddTween(transform, transform.position, transform.position + new Vector3(currentDir.x, currentDir.y, 0), speed);
        }

        if (transform.position.x >= Grid.instance.GridWorldSize.x - 0.5f)
        {
            transform.position = new Vector3(0, transform.position.y);
            Tweener._Instance.CancelTween(transform);
            Tweener._Instance.AddTween(transform, transform.position, transform.position + new Vector3(currentDir.x, currentDir.y, 0), speed);
        }
    }

    private bool CheckCollision(Vector2 dir)
    {

        RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(0.9f, 0.9f), 0, dir, 0.1f);//Physics2D.Raycast(transform.position, dir, 0.5f);
        if (hit)
            return true;

        return false;
    }
}

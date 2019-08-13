using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{

    [SerializeField] private float speedMult = 60;
    [SerializeField] private int targetFPS = 60;

    private Vector2 currentDir;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = targetFPS;
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

        transform.Translate(currentDir * speedMult * Time.deltaTime);
    }

    private bool CheckCollision(Vector2 dir)
    {
        Debug.Log(dir);

        RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(0.9f, 0.9f), 0, dir, 0.1f);//Physics2D.Raycast(transform.position, dir, 0.5f);
        if (hit)
            return true;

        return false;
    }
}

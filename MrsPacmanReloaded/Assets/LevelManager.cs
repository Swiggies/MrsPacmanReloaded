using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    [SerializeField] private Texture2D level;
    [SerializeField] private GameObject wallObj;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(level.width * 0.5f, level.height * 0.5f, -10);

        for (int x = 0; x < level.width; x++)
        {
            for (int y = 0; y < level.height; y++)
            {
                if (level.GetPixel(x, y) != Color.white)
                    Instantiate(wallObj, new Vector2(x, y), Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

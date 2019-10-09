using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [System.Serializable]
    private class ColorGameObject
    {
        public Color Color;
        public GameObject GameObject;
    }

    private Camera mainCam;
    [SerializeField] private Texture2D level;
    [SerializeField] private GameObject wallObj;
    [SerializeField] private GameObject smallPelletObj;
    [SerializeField] private ColorGameObject[] ColorsToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        mainCam.orthographicSize = (level.height / 2) + 3;
        mainCam.transform.position = new Vector3(level.width * 0.5f, (level.height * 0.5f) - 0.5f, -10);
        for (int x = 0; x < level.width; x++)
        {
            for (int y = 0; y < level.height; y++)
            {
                Color pixelColor = level.GetPixel(x, y);
                if (pixelColor == Color.black)
                    Instantiate(wallObj, new Vector2(x, y), Quaternion.identity);
                else if (pixelColor == Color.white)
                {
                    Instantiate(smallPelletObj, new Vector2(x, y), Quaternion.identity);
                }
                else
                {
                    for (int i = 0; i < ColorsToSpawn.Length; i++)
                    {
                        if(pixelColor == ColorsToSpawn[i].Color)
                            Instantiate(ColorsToSpawn[i].GameObject, new Vector2(x, y), Quaternion.identity);
                    }
                }
            }
        }
    }
}

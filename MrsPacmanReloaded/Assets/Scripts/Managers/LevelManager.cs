using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [System.Serializable]
    private class ColorGameObject
    {
        public Color Color;
        public GameObject GameObject;
    }

    [SerializeField] private GameObject[] spawnablePowerups;

    private Camera mainCam;
    [SerializeField] private Texture2D[] levels;
    [SerializeField] private GameObject wallObj;
    [SerializeField] private GameObject smallPelletObj;
    [SerializeField] private ColorGameObject[] ColorsToSpawn;

    private int spawnedPellets;

    public delegate void LevelLoadHandler();
    public static event LevelLoadHandler OnLevelLoad;

    // Start is called before the first frame update
    void Start()
    {
        GenerateLevel();

        Collectable.OnCollectablePickup += OnCollectablePickup;
        GameManager.OnGameRestart += OnGameRestart;
    }

    void GenerateLevel()
    {
        Texture2D level;
        if (SceneManager.GetActiveScene().name == "IterateLevel")
            level = levels[Random.Range(0, levels.Length)];
        else
            level = levels[0];


        mainCam = Camera.main;
        mainCam.orthographicSize = (level.height / 2) + 1;
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
                    spawnedPellets++;
                }
                else if(pixelColor == Color.green)
                {
                    int randomPowerup = Random.Range(0, spawnablePowerups.Length);
                    Instantiate(spawnablePowerups[randomPowerup], new Vector2(x, y), Quaternion.identity);
                }
                else
                {
                    for (int i = 0; i < ColorsToSpawn.Length; i++)
                    {
                        if (pixelColor == ColorsToSpawn[i].Color)
                            Instantiate(ColorsToSpawn[i].GameObject, new Vector2(x, y), Quaternion.identity);
                    }
                }
            }
        }
        OnLevelLoad?.Invoke();
        AStarGrid.Instance.CreateGrid();
    }

    private void OnGameRestart(GameManager.GameStates state)
    {
        if(state == GameManager.GameStates.Win || state == GameManager.GameStates.End)
            RestartLevel();
    }

    private void RestartLevel()
    {
        Destroy(GameObject.FindGameObjectWithTag("Player"));

        var levelParts = GameObject.FindGameObjectsWithTag("Level");
        for (int i = 0; i < levelParts.Length; i++)
        {
            Destroy(levelParts[i]);
        }
        GenerateLevel();
    }

    private void OnCollectablePickup(Collectable collectable)
    {
        if (collectable.CollectableType == Collectable.CollectableTypes.Pellet)
            spawnedPellets--;
        if (spawnedPellets <= 0)
            GameManager.Instance.GameWin();
    }
}

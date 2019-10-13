using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public static SceneManagement Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //GameManager.OnGameWin += OnGameWin;
    }

    private void OnGameWin()
    {
        //Invoke("RestartScene", 3);
    }

    public void RestartScene()
    {
        //var scene = SceneManager.GetActiveScene();
        //SceneManager.LoadScene(scene.name);
    }
}

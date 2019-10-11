using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const string HIGH_SCORE_KEY = "HighScore";
    public static string HighScoreKey { get { return HIGH_SCORE_KEY; } }

    public enum GameStates { End, Win, Running }

    public static GameManager Instance;
    public static GameStates GameState;
    public static int Score;
    public static bool GameStarted = false;
    public static int Lives = 3;

    public delegate void ScoreHandler();
    public static event ScoreHandler OnScoreChange;

    public delegate void RestartHandler(GameStates gameState);
    public static event RestartHandler OnGameRestart;

    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private GameObject gameWinCanvas;

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Collectable.OnCollectablePickup += OnCollectablePickup;
        GhostAI.OnDeath += OnGhostDeath;
        Invoke("StartGame", 3);
    }

    public void StartGame()
    {
        GameStarted = true;
        GameState = GameStates.Running;
    }

    private void OnGhostDeath()
    {
        Score += 100;
        OnScoreChange?.Invoke();
    }

    private void OnCollectablePickup(Collectable collectable)
    {
        Score += collectable.Score;
        OnScoreChange?.Invoke();
    }

    [ContextMenu("End")]
    public void EndGame()
    {
        Lives--;
        GameStarted = false;
        
        if (Lives > 0)
            Invoke("RestartGame", 3);
        else
        {
            if(Score > PlayerPrefs.GetInt(HIGH_SCORE_KEY))
                PlayerPrefs.SetInt(HIGH_SCORE_KEY, Score);

            GameState = GameStates.End;
            // show menu
            gameOverCanvas.SetActive(true);
            Score = 0;
        }
        //OnGameRestart(GameState);
    }

    [ContextMenu("Win")]
    public void GameWin()
    {
        if (Score > PlayerPrefs.GetInt(HIGH_SCORE_KEY))
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, Score);

        GameState = GameStates.Win;
        //Invoke("RestartGame", 3);
        gameWinCanvas.SetActive(true);
        GameStarted = false;
    }

    public void RestartGame()
    {
        if(GameState == GameStates.End && Lives <= 0)
        {
            Lives = 3;
        }
        OnGameRestart?.Invoke(GameState);
        Invoke("StartGame", 3);
        GameState = GameStates.Running;
    }
}

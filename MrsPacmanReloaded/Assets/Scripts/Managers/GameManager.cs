using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Used for the playerprefs to get/set high score
    private const string HIGH_SCORE_KEY = "HighScore";
    public static string HighScoreKey { get { return HIGH_SCORE_KEY; } }

    // Game state
    public enum GameStates { End, Win, Running }

    // Static memebrs used all over the project
    public static GameManager Instance;
    public static GameStates GameState;
    public static int Score;
    public static bool GameStarted = false;
    public static int Lives = 3;
    public static bool IterateLevel;

    // Handles the score
    public delegate void ScoreHandler();
    public static event ScoreHandler OnScoreChange;

    // Handles the restarting of games
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
        // Setting up events
        Collectable.OnCollectablePickup += OnCollectablePickup;
        GhostAI.OnDeath += OnGhostDeath;
        Invoke("StartGame", 3);

        if (SceneManager.GetActiveScene().name == "IterateLevel")
            IterateLevel = true;
    }

    // When the game is actually started
    public void StartGame()
    {
        GameStarted = true;
        GameState = GameStates.Running;
    }

    // When a ghost dies, update the score
    private void OnGhostDeath()
    {
        Score += 100;
        OnScoreChange?.Invoke();
    }

    // When a collectable is picked up
    // Update the score
    private void OnCollectablePickup(Collectable collectable)
    {
        Score += collectable.Score;
        OnScoreChange?.Invoke();
    }

    // Handle the game ending
    // This is run whenever the player dies
    // When they run out of lives the game is over
    [ContextMenu("End")]
    public void EndGame()
    {
        Lives--;
        GameStarted = false;
        
        // If the player still has lives, restart the game in 3 seconds
        if (Lives > 0)
            Invoke("RestartGame", 3);
        // If the player is out of lives
        // Update the high score and set the game state
        // Show the game over canvas
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

    // Handles the player winning
    // Run when the player has collected all pellets on the map
    [ContextMenu("Win")]
    public void GameWin()
    {
        // If the score the player has is higher than the high score, update the high score with PlayerPrefs
        if (Score > PlayerPrefs.GetInt(HIGH_SCORE_KEY))
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, Score);

        GameState = GameStates.Win;
        //Invoke("RestartGame", 3);
        gameWinCanvas.SetActive(true);
        GameStarted = false;
    }

    // Restart the game
    // Give the player the lives back
    // Reset the state
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

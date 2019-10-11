using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI livesText;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.OnScoreChange += OnScoreChange;
        highScoreText.text = PlayerPrefs.GetInt(GameManager.HighScoreKey).ToString("D7");
        GameManager.OnGameRestart += OnGameRestart;
    }

    private void OnGameRestart(GameManager.GameStates state)
    {
        livesText.text = "x" + GameManager.Lives.ToString();
        scoreText.text = scoreText.text = GameManager.Score.ToString("D7");
        highScoreText.text = PlayerPrefs.GetInt(GameManager.HighScoreKey).ToString("D7");
    }

    private void OnScoreChange()
    {
        scoreText.text = GameManager.Score.ToString("D7");
    }
}

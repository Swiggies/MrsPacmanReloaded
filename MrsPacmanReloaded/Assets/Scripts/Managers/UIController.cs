using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{

    // Private members
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI powerupText;

    // Start is called before the first frame update
    // Events are setup
    // Updated texts based on what level is loaded
    void Start()
    {
        GameManager.OnScoreChange += OnScoreChange;
        highScoreText.text = PlayerPrefs.GetInt(GameManager.HighScoreKey).ToString("D7");
        GameManager.OnGameRestart += OnGameRestart;

        if (GameManager.IterateLevel)
        {
            Collectable.OnCollectablePickup += OnPowerupPickup;
            Powerup.OnPowerupUsed += OnPowerupUsed;
        }
    }

    // When a powerup is used, update the Powerup text
    private void OnPowerupUsed()
    {
        powerupText.text = "NONE";
    }

    // When a powerup is picked up, update the powerup text
    private void OnPowerupPickup(Collectable collectable)
    {
        if (collectable.CollectableType == Collectable.CollectableTypes.Powerup)
            powerupText.text = collectable.SelectedPowerup.PowerupName.ToUpper();
    }

    // When the game is restart, update all the score texts
    private void OnGameRestart(GameManager.GameStates state)
    {
        livesText.text = "x" + GameManager.Lives.ToString();
        scoreText.text = scoreText.text = GameManager.Score.ToString("D7");
        highScoreText.text = PlayerPrefs.GetInt(GameManager.HighScoreKey).ToString("D7");
    }

    // When the score changes, update the score texxt
    private void OnScoreChange()
    {
        scoreText.text = GameManager.Score.ToString("D7");
    }
}

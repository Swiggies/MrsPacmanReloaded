using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.OnScoreChange += OnScoreChange;
    }

    private void OnScoreChange()
    {
        scoreText.text = GameManager.score.ToString("D7");
    }
}

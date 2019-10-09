using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static int score;

    public delegate void ScoreHandler();
    public static event ScoreHandler OnScoreChange;

    // Start is called before the first frame update
    void Start()
    {
        Collectable.OnCollectablePickup += OnCollectablePickup;
        GhostAI.OnDeath += OnGhostDeath;
    }

    private void OnGhostDeath()
    {
        score += 100;
        OnScoreChange?.Invoke();
    }

    private void OnCollectablePickup(Collectable collectable)
    {
        score += collectable.Score;
        OnScoreChange?.Invoke();
    }
}

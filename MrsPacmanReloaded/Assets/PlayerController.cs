using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const string ANIM_DEAD = "Dead";

    public bool IsDead;
    [SerializeField] private Animator animator;
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        GameManager.OnGameRestart += OnGameRestart;
    }

    private void OnGameRestart(GameManager.GameStates state)
    {
        transform.position = startPos;
        IsDead = false;
        animator.SetBool(ANIM_DEAD, IsDead);
        GetComponent<CircleCollider2D>().enabled = true;
    }

    public void KillPlayer()
    {
        if (IsDead)
            return;

        IsDead = true;
        animator.SetBool(ANIM_DEAD, IsDead);
        GetComponent<AudioSource>().Play();
        GetComponent<CircleCollider2D>().enabled = false;
        GameManager.Instance.EndGame();
    }
}

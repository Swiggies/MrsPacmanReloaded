using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const string ANIM_DEAD = "Dead";

    public bool IsDead;
    public MovementController PlayerMovementController { get; private set; }

    [SerializeField] private Animator animator;
    private Vector3 startPos;
    private Powerup currentPowerup;


    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        PlayerMovementController = GetComponent<MovementController>();
        GameManager.OnGameRestart += OnGameRestart;
        Collectable.OnCollectablePickup += OnPowerupPickup;
    }

    private void OnPowerupPickup(Collectable collectable)
    {
        if (!GameManager.IterateLevel)
            return;

        if(collectable.CollectableType == Collectable.CollectableTypes.Powerup)
        {
            currentPowerup = Instantiate(collectable.SelectedPowerup);
            currentPowerup.InitializePowerup(this);
        }
    }

    private void Update()
    {
        if(currentPowerup != null)
        {
            Debug.Log(currentPowerup.Active);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentPowerup.UsePowerup();
            }

            if (currentPowerup.Active)
                currentPowerup.PowerupUpdate();
        }
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

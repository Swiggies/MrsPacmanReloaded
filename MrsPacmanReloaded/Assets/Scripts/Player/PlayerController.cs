using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Key used for animation
    private const string ANIM_DEAD = "Dead";

    public bool IsDead;
    public MovementController PlayerMovementController { get; private set; }

    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip usePowerupClip;   
    private Vector3 startPos;
    private Powerup currentPowerup;
    private AudioSource audioSource;
    private CircleCollider2D circleCollider;

    // Start is called before the first frame update
    // Set up the player
    void Start()
    {
        startPos = transform.position;
        PlayerMovementController = GetComponent<MovementController>();
        GameManager.OnGameRestart += OnGameRestart;
        Collectable.OnCollectablePickup += OnPowerupPickup;
        if(GameManager.IterateLevel)
            Powerup.OnPowerupEnd += OnPowerupEnd;

        audioSource = GetComponent<AudioSource>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    // Unsubscribe from events when the player is killed
    private void OnDisable()
    {
        Powerup.OnPowerupEnd -= OnPowerupEnd;
        Collectable.OnCollectablePickup -= OnPowerupPickup;
        GameManager.OnGameRestart -= OnGameRestart;
    }

    // WHen the powerup used has ended
    // Destroy it
    private void OnPowerupEnd()
    {
        //currentPowerup = null;
        Destroy(currentPowerup);
    }

    // When a powerup is picked up
    // Instantiate it and initialize it
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

    // If the player presses space, use the current powerup
    // If the powerup has an update loop, run that
    private void Update()
    {
        if (!GameManager.IterateLevel)
            return;

        if(currentPowerup != null)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentPowerup.PowerupUse();
                audioSource.PlayOneShot(usePowerupClip);
            }

            if (currentPowerup.Active)
                currentPowerup?.PowerupUpdate();
        }
    }

    // When the game is restart
    // Reset the position and animation of the player
    private void OnGameRestart(GameManager.GameStates state)
    {
        transform.position = startPos;
        IsDead = false;
        animator.SetBool(ANIM_DEAD, IsDead);
        circleCollider.enabled = true;
    }

    // When the player is killed, run the animation, audio and disable any collisions so this isn't called multiple times
    // Let the GameManager know the game has finished
    public void KillPlayer()
    {
        if (IsDead)
            return;

        IsDead = true;
        animator.SetBool(ANIM_DEAD, IsDead);
        audioSource.Play();
        circleCollider.enabled = false;
        GameManager.Instance.EndGame();
    }
}

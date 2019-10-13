using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class handles all collectables in the game
public class Collectable : MonoBehaviour
{
    // Defines the type of collectable
    public enum CollectableTypes { Pellet, Powerup }

    public CollectableTypes CollectableType;
    public int Score;
    public Powerup SelectedPowerup;

    // The powerups that are used within the game
    // Only the first one is used when the game is on the RecreateLevel
    [SerializeField] private Powerup[] powerups;

    public delegate void CollectablePickupHandler(Collectable collectable);
    public static event CollectablePickupHandler OnCollectablePickup;

    // Setup the collectable
    private void Start()
    {
        if(CollectableType == CollectableTypes.Powerup && GameManager.IterateLevel)
            SelectedPowerup = powerups[Random.Range(0, powerups.Length)];

        if (!GameManager.IterateLevel)
            SelectedPowerup = powerups[0];
    }

    // Collision handling
    // Only reacts to the player
    public virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") || col.CompareTag("SuperPlayer"))
        {
            if (CollectableType == CollectableTypes.Powerup)
            {
                if (!GameManager.IterateLevel)
                    SelectedPowerup.PowerupUse();
            }

            OnCollectablePickup?.Invoke(this);
            Destroy(gameObject);
        }
    }
}

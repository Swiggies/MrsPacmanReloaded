using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public enum CollectableTypes { Pellet, Powerup }

    public CollectableTypes CollectableType;
    public int Score;
    public Powerup SelectedPowerup;

    [SerializeField] private Powerup[] powerups;

    public delegate void CollectablePickupHandler(Collectable collectable);
    public static event CollectablePickupHandler OnCollectablePickup;

    private void Start()
    {
        if(CollectableType == CollectableTypes.Powerup && GameManager.IterateLevel)
            SelectedPowerup = powerups[Random.Range(0, powerups.Length)];

        if (!GameManager.IterateLevel)
            SelectedPowerup = powerups[0];
    }

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

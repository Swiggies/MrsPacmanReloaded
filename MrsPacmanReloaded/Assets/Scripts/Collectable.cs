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
        if (!GameManager.IterateLevel)
            return;

        if(CollectableType == CollectableTypes.Powerup)
            SelectedPowerup = powerups[Random.Range(0, powerups.Length)];
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

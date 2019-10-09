using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public enum CollectableTypes { Pellet, Powerup }

    public CollectableTypes CollectableType;
    public int Score;

    public delegate void CollectablePickupHandler(Collectable collectable);
    public static event CollectablePickupHandler OnCollectablePickup;

    public virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            OnCollectablePickup?.Invoke(this);
            Destroy(gameObject);
        }
    }
}

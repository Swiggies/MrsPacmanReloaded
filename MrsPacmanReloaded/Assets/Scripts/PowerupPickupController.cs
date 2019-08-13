using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PowerupPickupController : MonoBehaviour
{
    public Powerup powerup;

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Powerup Picked up");
    }
}

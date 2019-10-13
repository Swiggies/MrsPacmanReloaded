using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Child of Powerup class
// Boosts the player in 1 direction
[CreateAssetMenu]
public class Boost : Powerup
{
    // Uses the players movementController to get/set the speed and direction
    private MovementController movementController;
    private Vector2 dir;

    // Initializes and sets up the boost
    public override void InitializePowerup(PlayerController _playerController)
    {
        base.InitializePowerup(_playerController);
        movementController = PlayerController.PlayerMovementController;
    }

    // WHen the powerup is used, it sets the speed of the player and changed their tag so they kill ghosts when touched
    // Locks the direction of the player
    public override void PowerupUse()
    {
        base.PowerupUse();
        dir = PlayerController.PlayerMovementController.CurrentDir;
        movementController.Speed = 0.05f;
        Active = true;
        movementController.LockedDir = true;
        PlayerController.gameObject.tag = "SuperPlayer";
    }

    // Called from PlayerController
    // Detects if anything in front of the player is hit
    // Stops boost if it hits anything
    public override void PowerupUpdate()
    {
        if (dir == Vector2.zero)
            PowerupEnd();

        RaycastHit2D hit = Physics2D.Raycast(movementController.transform.position, dir, 1, LayerMask.GetMask("Wall"));
        Debug.DrawRay(movementController.transform.position, dir);
        if (hit)
        {
            PowerupEnd();
        }
        base.PowerupUpdate();
    }

    // Called when the powerups ends
    // Resets the players speed and unlocks the direction
    public override void PowerupEnd()
    {
        movementController.Speed = MovementController.DefaultSpeed;
        movementController.LockedDir = false;
        PlayerController.gameObject.tag = "Player";
        
        Debug.Log("Boost Ended");
        base.PowerupEnd();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Boost : Powerup
{
    private MovementController movementController;
    private Vector2 dir;

    public override void InitializePowerup(PlayerController _playerController)
    {
        base.InitializePowerup(_playerController);
        movementController = PlayerController.PlayerMovementController;
    }

    public override void PowerupUse()
    {
        base.PowerupUse();
        dir = PlayerController.PlayerMovementController.CurrentDir;
        movementController.Speed = 0.05f;
        Active = true;
        movementController.LockedDir = true;
        PlayerController.gameObject.tag = "SuperPlayer";
    }

    public override void PowerupUpdate()
    {
        base.PowerupUpdate();
        RaycastHit2D hit = Physics2D.Raycast(movementController.transform.position, dir, 1, LayerMask.GetMask("Wall"));
        if (hit)
            PowerupEnd();
    }

    public override void PowerupEnd()
    {
        movementController.Speed = MovementController.DefaultSpeed;
        movementController.LockedDir = false;
        PlayerController.gameObject.tag = "Player";
        
        Debug.Log("Boost Ended");
        base.PowerupEnd();
    }
}

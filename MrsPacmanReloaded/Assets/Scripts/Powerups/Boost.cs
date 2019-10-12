using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Boost : Powerup
{
    private MovementController movementController;

    public override void InitializePowerup(PlayerController _playerController)
    {
        base.InitializePowerup(_playerController);
        movementController = PlayerController.PlayerMovementController;
    }

    public override void UsePowerup()
    {
        base.UsePowerup();
        movementController.Speed = 0.05f;
        Active = true;
        movementController.LockedDir = true;
        PlayerController.gameObject.tag = "SuperPlayer";
    }

    public override void PowerupUpdate()
    {
        base.PowerupUpdate();
        Debug.Log(movementController.CurrentDir);
        RaycastHit2D hit = Physics2D.Raycast(movementController.transform.position, movementController.CurrentDir, 1);
        if (hit)
            PowerupEnd();
    }

    public override void PowerupEnd()
    {
        Debug.Log("Boost Ended");
        base.PowerupEnd();
        movementController.Speed = MovementController.DefaultSpeed;
        movementController.LockedDir = false;
        PlayerController.gameObject.tag = "Player";
        Destroy(this);
    }
}

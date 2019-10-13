using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Child of Powerup class
// Spawns a "blocking" wall when used
[CreateAssetMenu]
public class Blocker : Powerup
{
    [SerializeField] private GameObject blockerWall;

    // Called when Spacebar is pressed
    // Gets the players position and spawns a wall there
    // Updates the node grid that that wall is impassable
    public override void PowerupUse()
    {
        base.PowerupUse();
        var position = PlayerController.transform.position;
        position.x = (int)position.x;
        position.y = (int)position.y;
        AStarGrid.Instance.SetPointToWall(position);
        Instantiate(blockerWall, position, Quaternion.identity);
        PowerupEnd();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Blocker : Powerup
{
    [SerializeField] private GameObject blockerWall;

    public override void PowerupUse()
    {
        var position = PlayerController.transform.position;
        position.x = (int)position.x;
        position.y = (int)position.y;
        Grid.Instance.SetPointToWall(position);
        Instantiate(blockerWall, position, Quaternion.identity);
        PowerupEnd();
    }
}

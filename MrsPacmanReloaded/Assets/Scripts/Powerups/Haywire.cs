using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Child of Powerup class
// Makes all the ghosts go "haywire"
[CreateAssetMenu]
public class Haywire : Powerup
{
    // Events for the powerup
    // When used, all ghosts should go haywire
    public delegate void HaywireHandler();
    public static event HaywireHandler OnHaywire;

    // Calls the event and tells the PlayerController that the powerup has been used
    public override void PowerupUse()
    {
        base.PowerupUse();
        OnHaywire?.Invoke();
        PowerupEnd();
    }
}

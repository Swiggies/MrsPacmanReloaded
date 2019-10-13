using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Haywire : Powerup
{
    public delegate void HaywireHandler();
    public static event HaywireHandler OnHaywire;

    public override void PowerupUse()
    {
        OnHaywire?.Invoke();
        PowerupEnd();
    }
}

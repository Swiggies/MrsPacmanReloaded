using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : ScriptableObject
{
    public string powerupName;
    public string powerupDescription;
    
    public virtual void InitializePowerup()
    {
        Debug.Log("Powerup Initialized");
    }

    public virtual void UsePowerup()
    {
        Debug.Log("Powerup Used");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Powerup : ScriptableObject
{
    public string PowerupName;
    public bool Active;

    public PlayerController PlayerController { get; private set; }

    public delegate void PowerupUsedHandler();
    public static event PowerupUsedHandler OnPowerupUse;

    public virtual void InitializePowerup(PlayerController _playerController)
    {
        this.PlayerController = _playerController;
    }

    public virtual void UsePowerup() { OnPowerupUse?.Invoke(); }

    public virtual void PowerupUpdate() { }

    public virtual void PowerupEnd() { }
}

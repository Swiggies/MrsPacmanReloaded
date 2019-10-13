using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class handles the powerups
public abstract class Powerup : ScriptableObject
{
    // Public fields
    public string PowerupName;
    // Active is used to tell the PlayerController it has been used and should update PowerupUpdate();
    public bool Active;
    public PlayerController PlayerController { get; private set; }

    // Events and handlers
    public delegate void PowerupUseHandler();
    public static event PowerupUseHandler OnPowerupUsed;

    public delegate void PowerupEndHandler();
    public static event PowerupEndHandler OnPowerupEnd;

    // Powerups all get initialized when picked up and are passed the PlayerController
    public virtual void InitializePowerup(PlayerController _playerController)
    {
        this.PlayerController = _playerController;
    }

    // Overrideen by the child classes
    public virtual void PowerupUse() { OnPowerupUsed?.Invoke(); }

    // Overrideen by the child classes
    public virtual void PowerupUpdate() { }

    // Overrideen by the child classes
    public virtual void PowerupEnd() { OnPowerupEnd?.Invoke(); }
}

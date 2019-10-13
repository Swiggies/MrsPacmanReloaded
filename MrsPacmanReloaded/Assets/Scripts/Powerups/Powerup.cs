using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Powerup : ScriptableObject
{
    public string PowerupName;
    public bool Active;

    public PlayerController PlayerController { get; private set; }

    public delegate void PowerupUseHandler();
    public static event PowerupUseHandler OnPowerupUsed;

    public delegate void PowerupEndHandler();
    public static event PowerupEndHandler OnPowerupEnd;

    public virtual void InitializePowerup(PlayerController _playerController)
    {
        this.PlayerController = _playerController;
    }

    public virtual void PowerupUse() { OnPowerupUsed?.Invoke(); }

    public virtual void PowerupUpdate() { }

    public virtual void PowerupEnd() { OnPowerupEnd?.Invoke(); }
}

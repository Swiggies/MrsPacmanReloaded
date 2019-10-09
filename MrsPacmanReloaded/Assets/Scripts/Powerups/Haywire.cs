using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Haywire : Collectable
{
    public delegate void HaywireHandler();
    public static event HaywireHandler OnHaywire;

    public override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Debug.Log("test???");
            OnHaywire?.Invoke();
            base.OnTriggerEnter2D(col);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField] private AudioClip pelletPickupClip;
    [SerializeField] private AudioClip powerupPickupClip;
    [SerializeField] private AudioSource audioSource;

    [Header("Music")]
    [SerializeField] private AudioClip musicClip;
    [SerializeField] private AudioSource musicAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        Collectable.OnCollectablePickup += OnCollectablePickup;
    }

    private void OnCollectablePickup(Collectable collectable)
    {
        if (collectable.CollectableType == Collectable.CollectableTypes.Pellet)
        {
            audioSource.PlayOneShot(pelletPickupClip);
        }
        if (collectable.CollectableType == Collectable.CollectableTypes.Powerup)
        {
            audioSource.PlayOneShot(powerupPickupClip);
        }
    }
}

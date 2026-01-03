using System;
using UnityEngine;

public class PlayOnHit : MonoBehaviour
{
    public Hurtbox hurtbox;
    private AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
        if (hurtbox != null)
        {
            hurtbox.onHitTriggerEvent += OnHit;
        }
        else
        {
            Debug.LogWarning("Warning: hurtbox is null");
        }
    }

    private void OnHit(Hitbox box)
    {
        this.source.Play();
    }
}

using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 3;
    public Hurtbox hurtbox;
    public bool isDestroyedOnDeath;

    void Start()
    {
        if (hurtbox != null)
        {
            hurtbox.onHitTriggerEvent += OnHit;
        }
        else
        {
            Debug.LogWarning("Warning: hurtbox is null");
        }
    }

    void OnDisable()
    {
        if (hurtbox != null)
        {
            hurtbox.onHitTriggerEvent -= OnHit;
        }
        else
        {
            Debug.LogWarning("Hurtbox is null");
        }
    }

    void OnHit(Hitbox box)
    {
        health -= 1;
        if (health < 1 && isDestroyedOnDeath){
            Destroy(gameObject);
        }
    }

}

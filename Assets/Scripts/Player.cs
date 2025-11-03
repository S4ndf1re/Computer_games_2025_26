using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 3;
    public Hurtbox hurtbox;

    void Start()
    {
        if (hurtbox != null)
        {
            hurtbox.onHitTriggerEvent += OnHit;
        }
        else
        {
            Debug.Log("Warning: hurtbox is null");
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
            Debug.Log("Warning: hurtbox is null");
        }
    }

    void OnHit(Hitbox box)
    {
        health -= 1;
        if (health < 1) {
            Destroy(gameObject);
        }
    }

}

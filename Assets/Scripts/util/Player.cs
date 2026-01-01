using System.Data;
using UnityEngine;

public class Player : MonoBehaviour
{
    public delegate void RespawnedDelegate();
    public event RespawnedDelegate playerRespawned;

    public StatTracker statTracker;

    public int startHealth = 3;
    public Hurtbox hurtbox;
    public bool isDestroyedOnDeath;

    public int currentHealth = 3;

    void Start()
    {
        currentHealth = startHealth;
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
        int damage = box.damage;
        currentHealth -= damage;
        
        if (statTracker != null)
        {
            statTracker.RegisterHit();
            statTracker.RegisterDamageTaken(damage);
        }

        if (currentHealth < 1 && isDestroyedOnDeath)
        {
            Destroy(gameObject);
        }
    }

    public void Heal()
    {
        currentHealth = startHealth;
    }

    public void Heal(int amount)
    {
        int newValue = currentHealth + amount;
        if (newValue > startHealth)
        {
            currentHealth = newValue;
        } else
        {
            currentHealth = startHealth;
        }
    }

    public void Respawn()
    {
        this.currentHealth = startHealth;
        playerRespawned?.Invoke();
    }

    public int GetHealth()
    {
        return this.currentHealth;
    }

    public bool IsDead()
    {
        return this.currentHealth <= 0;
    }

}

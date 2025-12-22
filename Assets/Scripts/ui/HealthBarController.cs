using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public Slider healthSlider;
    public Slider easeSlider;
    public Player player;
    // Set this to players max health
    public int maxHealth;
    public float health;
    public float healthRatio;
    public float lerpSpeed = 0.05f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHealth = player.startHealth;
        health = maxHealth;
        healthRatio = health / maxHealth * 100;
    }

    // Update is called once per frame
    void Update()
    {
        //Prozentualen anteil ausrechnen
        health = player.GetHealth();
        healthRatio = health / maxHealth * 100;

        if(healthSlider.value != healthRatio)
        {
            healthSlider.value = healthRatio;
        }

        if(easeSlider.value != healthRatio)
        {
            easeSlider.value = Mathf.Lerp(easeSlider.value, healthRatio, lerpSpeed);
        }
    }
}

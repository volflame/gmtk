using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log($"Boss health: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Boss defeated");
        // trigger death sequence/animation here later
    }

    public float HealthPercent => currentHealth / maxHealth;
}
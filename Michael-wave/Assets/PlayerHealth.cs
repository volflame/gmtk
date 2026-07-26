using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public HealthUI healthUI;
    public PlayerController playerController;
    public float defaultKnockbackDuration = 0.2f;

    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        healthUI.SetHealth(currentHealth);

        if (playerController == null)
        {
            playerController = GetComponent<PlayerController>();
        }
    }

    // Original signature — kept for anything already calling this (e.g. your K-key test)
    public void TakeDamage(int amount)
    {
        currentHealth = Mathf.Max(0, currentHealth - amount);
        healthUI.SetHealth(currentHealth);
    }

    // New overload — use this for anything that should knock the player back
    public void TakeDamage(int amount, Vector2 knockbackDirection, float knockbackForce)
    {
        currentHealth = Mathf.Max(0, currentHealth - amount);
        healthUI.SetHealth(currentHealth);

        if (playerController != null)
        {
            playerController.ApplyKnockback(knockbackDirection, knockbackForce, defaultKnockbackDuration);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(1);
        }
    }
}
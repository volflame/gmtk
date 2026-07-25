using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public HealthUI healthUI;

    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        healthUI.SetHealth(currentHealth);
    }

    public void TakeDamage(int amount)
    {
        currentHealth = Mathf.Max(0, currentHealth - amount);
        healthUI.SetHealth(currentHealth);
    }

    void Update()
    {
        // TEMPORARY: sanity-check key for the health UI, remove once enemy contact deals real damage
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(1);
        }
    }
}

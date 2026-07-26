using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public HealthUI healthUI;
    public PlayerController playerController;

    [Header("Invincibility")]
    public float invincibilityDuration = 0.5f;
    public float flashInterval = 0.08f;
    public SpriteRenderer spriteRenderer;

    [Header("Death/Restart")]
    public bool isDead = false;

    private int currentHealth;
    private float invincibleTimer = 0f;

    public bool IsInvincible => invincibleTimer > 0f;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
        healthUI.SetHealth(currentHealth);
    }

    void Update()
    {
        if (isDead)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartScene();
            }
            return; // no other input matters once dead
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(1);
        }

        if (invincibleTimer > 0f)
        {
            invincibleTimer -= Time.deltaTime;
        }
    }

    public void TakeDamage(int amount, Vector2 direction, float knockbackForce)
    {
        if (isDead || invincibleTimer > 0f) return;

        invincibleTimer = invincibilityDuration;

        if (playerController != null && knockbackForce > 0f)
        {
            playerController.ApplyKnockback(direction, knockbackForce);
        }

        currentHealth = Mathf.Max(0, currentHealth - amount);
        healthUI.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        if (spriteRenderer != null)
        {
            StopCoroutine(nameof(FlashDuringInvincibility));
            StartCoroutine(nameof(FlashDuringInvincibility));
        }
    }

    public void TakeDamage(int amount)
    {
        TakeDamage(amount, Vector2.zero, 0f);
    }

    private void Die()
{
    isDead = true;

    if (GameOverManager.Instance != null)
    {
        GameOverManager.Instance.TriggerGameOver();
    }

    Destroy(gameObject);
}

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator FlashDuringInvincibility()
    {
        float elapsed = 0f;
        while (elapsed < invincibilityDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(flashInterval);
            elapsed += flashInterval;
        }
        spriteRenderer.enabled = true;
    }
}
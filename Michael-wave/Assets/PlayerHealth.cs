using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public HealthUI healthUI;

    [Header("Invincibility")]
    public float invincibilityDuration = 0.5f;
    public float flashInterval = 0.08f;
    public SpriteRenderer spriteRenderer;

    private int currentHealth;
    private float invincibleTimer = 0f;
    private PlayerController playerController;

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
        if (invincibleTimer > 0f)
        {
            invincibleTimer -= Time.deltaTime;
        }
    }

    // Contact damage: knocks the player away from the damage source and starts i-frames.
    public void TakeDamage(int amount, Vector2 direction, float knockbackForce)
    {
        if (invincibleTimer > 0f) return; // already in i-frames, ignore

        invincibleTimer = invincibilityDuration;

        if (playerController != null && knockbackForce > 0f)
        {
            playerController.ApplyKnockback(direction, knockbackForce);
        }

        currentHealth = Mathf.Max(0, currentHealth - amount);
        healthUI.SetHealth(currentHealth);

        if (spriteRenderer != null)
        {
            StopCoroutine(nameof(FlashDuringInvincibility));
            StartCoroutine(nameof(FlashDuringInvincibility));
        }
    }

    // Kept so anything calling the old single-argument form still compiles.
    public void TakeDamage(int amount)
    {
        TakeDamage(amount, Vector2.zero, 0f);
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
        spriteRenderer.enabled = true; // never leave the player invisible
    }
}

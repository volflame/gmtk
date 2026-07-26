using System.Collections;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Hit Feedback")]
    public Material hitFlashMaterial;
    public float hitFlashDuration = 0.18f;

    // The boss is made of several sprite parts (body, hands, fists, pose variants),
    // so the whole thing flashes rather than just one renderer.
    private SpriteRenderer[] spriteRenderers;
    private Material[] originalMaterials;
    private Coroutine hitFlashRoutine;

    void Awake()
    {
        currentHealth = maxHealth;

        // include inactive: the pose/fist variants get toggled on and off at runtime
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);
        originalMaterials = new Material[spriteRenderers.Length];
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            originalMaterials[i] = spriteRenderers[i].sharedMaterial;
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log($"Boss health: {currentHealth}/{maxHealth}");

        FlashOnHit();

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

    private void FlashOnHit()
    {
        if (hitFlashMaterial == null || spriteRenderers == null || spriteRenderers.Length == 0) return;

        if (hitFlashRoutine != null) StopCoroutine(hitFlashRoutine);
        hitFlashRoutine = StartCoroutine(HitFlash());
    }

    private IEnumerator HitFlash()
    {
        foreach (SpriteRenderer renderer in spriteRenderers)
        {
            if (renderer != null) renderer.material = hitFlashMaterial;
        }

        yield return new WaitForSeconds(hitFlashDuration);

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            if (spriteRenderers[i] != null) spriteRenderers[i].material = originalMaterials[i];
        }

        hitFlashRoutine = null;
    }

    public float HealthPercent => currentHealth / maxHealth;
}

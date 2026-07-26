using System;
using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Shared Movement")]
    public float moveSpeed = 2f;
    protected Rigidbody2D rb;
    protected Transform target;
    protected Vector2 moveDirection;

    [Header("Shared Health")]
    protected float health;
    public float maxHealth = 3f;

    protected bool isKnocked = false;

    private float speedMultiplier = 1f;
    private int slowStacks = 0; // handles overlapping slow zones cleanly
    public Animator animator;

    [Header("Contact Damage")]
    public int contactDamage = 1;
    public float contactKnockback = 12f;

    [Header("Hit Feedback")]
    public Material hitFlashMaterial;
    public float hitFlashDuration = 0.18f;
    private Material originalMaterial;
    private Coroutine hitFlashRoutine;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Not every enemy prefab has this wired in the Inspector, so resolve it here.
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
        if (spriteRenderer != null)
        {
            originalMaterial = spriteRenderer.sharedMaterial;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryDamagePlayer(collision.collider);
    }

    // Also fires while the enemy stays pressed against the player, so contact damage
    // resumes once the player's invincibility window expires.
    private void OnCollisionStay2D(Collision2D collision)
    {
        TryDamagePlayer(collision.collider);
    }

    private void TryDamagePlayer(Collider2D other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth == null || playerHealth.IsInvincible) return;

        Vector2 direction = (other.transform.position - transform.position).normalized;
        playerHealth.TakeDamage(contactDamage, direction, contactKnockback);
    }

    protected virtual void Start()
    {
        target = GameObject.Find("Player").transform;
        health = maxHealth;
    }

    protected virtual void Update()
    {
        if (target)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            moveDirection = direction;

            // float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            // rb.rotation = angle;

            UpdateAnimatorDirection(direction);
        }

        OnUpdate(); // hook for subclass-specific per-frame logic (e.g. fire timers)
    }
    public SpriteRenderer spriteRenderer; // add this

    protected void UpdateAnimatorDirection(Vector3 direction)
    {
        if (animator == null) return;

        animator.SetFloat("Speed", rb.linearVelocity.magnitude);

        if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
        {
            animator.SetInteger("Direction", direction.y > 0 ? 1 : 0); // Back : Front
        }
        else
        {
            animator.SetInteger("Direction", 2); // Side
            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = direction.x > 0; // mirror when facing left
            }
        }
    }

    private void FixedUpdate()
    {
        if (isKnocked)
        {
            HandleKnockbackRecovery();
        }
        else
        {
            Move(); // subclass-specific movement/positioning logic
        }
    }

    // Subclasses implement their own movement behavior here
    protected abstract void Move();

    // Optional hook for subclasses that need per-frame logic outside movement (e.g. shooting cooldowns)
    protected virtual void OnUpdate() { }

    protected virtual void HandleKnockbackRecovery()
    {
        if (rb.linearVelocity.magnitude < 0.1f)
        {
            rb.linearVelocity = Vector2.zero;
            isKnocked = false;
        }
    }

    public void TakeDamage(float damage, Vector2 direction, float knockbackForce)
    {
        health -= damage;
        KnockBack(direction, knockbackForce);
        FlashOnHit();
        if (health <= 0)
        {
            Die(); // add coroutine and animation here later on ryan
        }
    }

    private void FlashOnHit()
    {
        if (spriteRenderer == null || hitFlashMaterial == null) return;

        if (hitFlashRoutine != null) StopCoroutine(hitFlashRoutine);
        hitFlashRoutine = StartCoroutine(HitFlash());
    }

    private IEnumerator HitFlash()
    {
        spriteRenderer.material = hitFlashMaterial;
        yield return new WaitForSeconds(hitFlashDuration);
        if (spriteRenderer != null)
        {
            spriteRenderer.material = originalMaterial;
        }
        hitFlashRoutine = null;
    }

    protected virtual void Die()
    {
        if (WaveSpawner.Instance != null)
        {
            WaveSpawner.Instance.EnemyDied();
        }

        Destroy(gameObject);
    }

    public void KnockBack(Vector2 direction, float knockbackForce)
    {
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
        isKnocked = true;
    }

    public void ApplySlow(float multiplier)
    {
        slowStacks++;
        speedMultiplier = multiplier; // simplest: just use the latest slow applied
    }

    public void RemoveSlow()
    {
        slowStacks--;
        if (slowStacks <= 0)
        {
            slowStacks = 0;
            speedMultiplier = 1f;
        }
    }

    protected float CurrentSpeed => moveSpeed * speedMultiplier;


}
using System;
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

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
        }

        OnUpdate(); // hook for subclass-specific per-frame logic (e.g. fire timers)
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
        if (health <= 0)
        {
            Die(); // add coroutine and animation here later on ryan
        }
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
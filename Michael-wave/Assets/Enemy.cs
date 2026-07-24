using System.Collections;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.TextCore;
using System;

public class Enemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float moveSpeed = 2f;
    Rigidbody2D rb;
    Transform target;
    Vector2 moveDirection;

    float health, maxHealth = 3f;

    private enum MovementState
    {
        Free,
        Knocked,
    }

    private MovementState movementState;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        target = GameObject.Find("Player").transform;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            moveDirection = direction;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
        }
    }

    private void FixedUpdate()
    {
        switch (movementState)
        {
            case MovementState.Free:
                if (target)
                {
                    rb.linearVelocity = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
                }
                break;
            case MovementState.Knocked:
                if (rb.linearVelocity.magnitude < 0.1f)
                {
                    rb.linearVelocity = Vector2.zero;
                    movementState = MovementState.Free;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
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

    void Die()
    {
        // Tell the wave spawner this enemy is gone
        if (WaveSpawner.Instance != null)
        {
            WaveSpawner.Instance.EnemyDied();
        }

        Destroy(gameObject);
    }

    // knockback implementation

    public void KnockBack(Vector2 direction, float knockbackForce)
    {
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
        movementState = MovementState.Knocked;
    }
}

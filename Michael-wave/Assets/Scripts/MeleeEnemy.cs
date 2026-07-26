using UnityEngine;

public class MeleeEnemy : Enemy
{
    [Header("Attack Settings")]
    public float attackRange = 1f;
    public float attackCooldown = 0.3f;
    public float attackDamage = 1f;
    public float attackKnockback = 5f;
    private float attackTimer = 0f;

    protected override void Move()
    {
        if (target)
        {
            rb.linearVelocity = moveDirection * CurrentSpeed;
        }
    }

    protected override void OnUpdate()
    {
        if (!target) return;

        attackTimer += Time.deltaTime;

        float distance = Vector2.Distance(transform.position, target.position);
        if (distance <= attackRange && attackTimer >= attackCooldown)
        {
            attackTimer = 0f;
            Attack();
        }
    }

    private void Attack()
    {
        if (animator != null)
        {
            animator.SetTrigger("Slice");
        }

        PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
        if (playerHealth != null && !playerHealth.IsInvincible)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            playerHealth.TakeDamage(Mathf.RoundToInt(attackDamage), direction, attackKnockback);
        }
    }
}
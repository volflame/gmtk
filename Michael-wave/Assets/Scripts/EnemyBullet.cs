using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float damage = 1;
    public float knockbackForce;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
        if (playerHealth != null && !playerHealth.IsInvincible)
        {
            Vector2 direction = (playerHealth.transform.position - transform.position).normalized;
            playerHealth.TakeDamage(Mathf.RoundToInt(damage), direction, knockbackForce);
        }

        if (gameObject.tag == "Projectile")
        {
            Destroy(gameObject);
        }
    }
}
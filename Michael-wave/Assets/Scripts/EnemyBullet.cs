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
<<<<<<< HEAD
        PlayerHealth player = collision.GetComponent<PlayerHealth>();
        if (player != null)
        {
            Vector2 direction = (collision.transform.position - transform.position).normalized;
            player.TakeDamage((int)damage, direction, knockbackForce);
=======
        PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
        if (playerHealth != null && !playerHealth.IsInvincible)
        {
            Vector2 direction = (playerHealth.transform.position - transform.position).normalized;
            playerHealth.TakeDamage(Mathf.RoundToInt(damage), direction, knockbackForce);
>>>>>>> 5234afddc4521ad3ccfdd2a5dcabb38c09a8fee3
        }

        if (gameObject.tag == "Projectile")
        {
            Destroy(gameObject);
        }
    }
}
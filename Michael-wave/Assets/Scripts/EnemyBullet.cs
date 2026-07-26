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
        PlayerHealth player = collision.GetComponent<PlayerHealth>();
        if (player != null)
        {
            Vector2 direction = (collision.transform.position - transform.position).normalized;
            player.TakeDamage((int)damage, direction, knockbackForce);
        }

        if (gameObject.tag == "Projectile")
        {
            Destroy(gameObject);
        }
    }
}
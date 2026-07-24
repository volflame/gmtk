using UnityEngine;

public class Bullet : MonoBehaviour
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
        // THIS IS WHERE YOU CHECK IF YOU'RE HITTING ENEMY
        // DAMAGE ENEMY
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            Vector2 direction = (enemy.transform.position - transform.position).normalized;
            enemy.TakeDamage(damage, direction, knockbackForce);
        }

        // Destroy(gameObject);
        if (gameObject.tag == "Projectile")
        {
            Destroy(gameObject);
        }

    }
}

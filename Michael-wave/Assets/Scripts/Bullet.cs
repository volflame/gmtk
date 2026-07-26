using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 1;
    public float knockbackForce;
    public float lifetime = 3f; // projectiles that never hit anything would otherwise live forever

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Only real projectiles expire. This same script also sits on melee hitboxes
        // (Burrito, Egg, Whip), which are Untagged and must not be destroyed.
        if (lifetime > 0f && gameObject.CompareTag("Projectile"))
        {
            Destroy(gameObject, lifetime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // THIS IS WHERE YOU CHECK IF YOU'RE HITTING ENEMY
        // DAMAGE ENEMY
        // Debug.Log($"Triggered by: {collision.gameObject.name}, layer: {LayerMask.LayerToName(collision.gameObject.layer)}");
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

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
        // THIS IS WHERE YOU CHECK IF YOU'RE HITTING PLAYER
        // DAMAGE PLAYER
        // Player player = collision.GetComponent<Player>();
        // if (player != null)
        // {
        //     Vector2 direction = (player.transform.position - transform.position).normalized;
        //     player.TakeDamage(damage, direction, knockbackForce);
        // }

        if (gameObject.tag == "Projectile")
        {
            Destroy(gameObject);
        }
    }
}
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // THIS IS WHERE YOU CHECK IF YOU'RE HITTING ENEMY
        // DAMAGE ENEMY
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        // Destroy(gameObject);
        if (gameObject.tag == "Projectile")
        {
            Destroy(gameObject);
        }

    }
}

using UnityEngine;

public class Boomerang : MonoBehaviour
{
    public float outSpeed = 15f;
    public float returnSpeed = 20f;
    public float outDuration = 0.4f; // time flying outward before returning
    public float damage = 1f;

    private Rigidbody2D rb;
    private Transform player;
    private float timer = 0f;
    private bool returning = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(Vector2 direction, Transform playerTransform)
    {
        player = playerTransform;
        rb.linearVelocity = direction.normalized * outSpeed;
    }

    private void Update()
    {
        if (!returning)
        {
            timer += Time.deltaTime;
            if (timer >= outDuration)
            {
                returning = true;
            }
        }
        else if (player != null)
        {
            Vector2 dirToPlayer = (player.position - transform.position).normalized;
            rb.linearVelocity = dirToPlayer * returnSpeed;

            // Destroy once it reaches the player
            if (Vector2.Distance(transform.position, player.position) < 0.5f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            Vector2 knockDir = (enemy.transform.position - transform.position).normalized;
            enemy.TakeDamage(damage, knockDir, 5f); // adjust knockback force as needed
        }
        // No self-destruct on hit — boomerang keeps flying/returning
    }
}
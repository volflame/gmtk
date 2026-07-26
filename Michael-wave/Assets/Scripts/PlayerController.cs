using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float moveSpeed;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    float speedX, speedY;
    Rigidbody2D rb;
    public Transform firePoint;

    [Header("Knockback")]
    public float knockbackDuration = 0.2f;
    private float knockbackTimer = 0f;
    public bool IsKnocked => knockbackTimer > 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Applies an impulse and suspends input-driven movement, otherwise the velocity
    // assignment below would overwrite the knockback on the very next frame.
    public void ApplyKnockback(Vector2 direction, float force)
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();

        knockbackTimer = knockbackDuration;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        if (knockbackTimer > 0f)
        {
            knockbackTimer -= Time.deltaTime;
            if (animator != null) animator.SetFloat("Speed", 0f);

            if (knockbackTimer <= 0f)
            {
                knockbackTimer = 0f;
                rb.linearVelocity = Vector2.zero;
            }
            return; // let the impulse carry the player; ignore input this frame
        }

        speedX = Input.GetAxisRaw("Horizontal") * moveSpeed;
        speedY = Input.GetAxisRaw("Vertical") * moveSpeed;
        rb.linearVelocity = new Vector2(speedX, speedY);

        // find the dominant direction for the bullet to fire
        Vector2 velocity = new Vector2(speedX, speedY);
        animator.SetFloat("Speed", velocity.magnitude);

        if (speedX != 0f || speedY != 0f)
        {
            Vector2 facingDir;

            if (Mathf.Abs(speedX) > Mathf.Abs(speedY))
                facingDir = new Vector2(Mathf.Sign(speedX), 0f);
            else
                facingDir = new Vector2(0f, Mathf.Sign(speedY));

            float angle = Mathf.Atan2(facingDir.y, facingDir.x) * Mathf.Rad2Deg;

            // Rotate ONLY the firePoint, not the player's visual transform
            firePoint.rotation = Quaternion.Euler(0f, 0f, angle - 90f);

            if (velocity.sqrMagnitude > 0.01f)
            {
                if (Mathf.Abs(speedY) > Mathf.Abs(speedX))
                {
                    animator.SetInteger("Direction", speedY > 0 ? 1 : 0); // Up : Down
                    spriteRenderer.flipX = false; // forward/back sprites are never mirrored
                }
                else
                {
                    animator.SetInteger("Direction", 2); // Side
                    spriteRenderer.flipX = speedX > 0; // mirror for left
                }
            }
        }

        // if (speedX != 0f || speedY != 0f)
        // {
        //     float angle = Mathf.Atan2(speedY, speedX) * Mathf.Rad2Deg;
        //     angle = Mathf.Round(angle / 45f) * 45f; // snap to nearest 45°
        //     transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        // }

        // if (Input.GetKeyDown(KeyCode.Z))
        // {
        //     weapon.Fire();
        // }

        // if (Input.GetKeyDown(KeyCode.X))
        // {
        //     weapon.MeleeAttack();
        // }

        // if (Input.GetKeyDown(KeyCode.C))
        // {
        //     weapon.SpecialFire();
        // }
    }
}

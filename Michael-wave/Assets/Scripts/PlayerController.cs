using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    float speedX, speedY;
    Rigidbody2D rb;
    public Transform firePoint;
    private bool isKnocked = false;
    private float knockbackTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isKnocked)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0f)
            {
                isKnocked = false;
            }
            return;
        }

        speedX = Input.GetAxisRaw("Horizontal") * moveSpeed;
        speedY = Input.GetAxisRaw("Vertical") * moveSpeed;
        rb.linearVelocity = new Vector2(speedX, speedY);

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

            firePoint.rotation = Quaternion.Euler(0f, 0f, angle - 90f);

            if (velocity.sqrMagnitude > 0.01f)
            {
                if (Mathf.Abs(speedY) > Mathf.Abs(speedX))
                {
                    animator.SetInteger("Direction", speedY > 0 ? 1 : 0); // Up : Down
                    spriteRenderer.flipX = false;
                }
                else
                {
                    animator.SetInteger("Direction", 2); // Side
                    spriteRenderer.flipX = speedX > 0;
                }
            }
        }
    }

    public void ApplyKnockback(Vector2 direction, float force, float duration)
    {
        rb.linearVelocity = direction.normalized * force;
        isKnocked = true;
        knockbackTimer = duration;
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
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float moveSpeed;
    public Weapon weapon;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    float speedX, speedY;
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        speedX = Input.GetAxisRaw("Horizontal") * moveSpeed;
        speedY = Input.GetAxisRaw("Vertical") * moveSpeed;
        rb.linearVelocity = new Vector2(speedX, speedY);

        Vector2 velocity = new Vector2(speedX, speedY);
        animator.SetFloat("Speed", velocity.magnitude);

        if (velocity.sqrMagnitude > 0.01f)
        {
            if (Mathf.Abs(speedY) > Mathf.Abs(speedX))
            {
                animator.SetInteger("Direction", speedY > 0 ? 1 : 0); // Up : Down
            }
            else
            {
                animator.SetInteger("Direction", 2); // Side
                spriteRenderer.flipX = speedX < 0; // mirror for left
            }
        }

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

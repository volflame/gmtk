using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float moveSpeed;
    public Weapon weapon;
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

        // find the dominant direction for the bullet to fire
        // if (speedX != 0f || speedY != 0f)
        // {
        //     Vector2 facingDir;

        //     if (Mathf.Abs(speedX) > Mathf.Abs(speedY))
        //         facingDir = new Vector2(Mathf.Sign(speedX), 0f);
        //     else
        //         facingDir = new Vector2(0f, Mathf.Sign(speedY));

        //     float angle = Mathf.Atan2(facingDir.y, facingDir.x) * Mathf.Rad2Deg;

        //     // Subtract 90 if your sprite's "forward" art faces UP by default.
        //     // Remove the "- 90f" if your sprite faces RIGHT by default.
        //     transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        // }

        if (speedX != 0f || speedY != 0f)
        {
            float angle = Mathf.Atan2(speedY, speedX) * Mathf.Rad2Deg;
            angle = Mathf.Round(angle / 45f) * 45f; // snap to nearest 45°
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
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

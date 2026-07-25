using UnityEngine;

public class RangedEnemy : Enemy
{
    [Header("Ranged Movement")]
    public float preferredRange = 5f;
    public float rangeBuffer = 0.5f;

    [Header("Shooting")]
    public GameObject projectilePrefab;
    public float projectileSpeed = 8f;
    public float fireCooldown = 1.5f;
    private float fireTimer = 0f;
    private bool holdingRange = false;

    protected override void Move()
    {
        if (!target)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float distance = Vector2.Distance(transform.position, target.position);

        if (distance > preferredRange + rangeBuffer)
        {
            rb.linearVelocity = moveDirection * CurrentSpeed; // move toward player
            holdingRange = false;
        }
        else if (distance < preferredRange - rangeBuffer)
        {
            rb.linearVelocity = -moveDirection * CurrentSpeed; // back away
            holdingRange = false;
        }
        else
        {
            rb.linearVelocity = Vector2.zero; // pause at range
            holdingRange = true;
        }
    }

    protected override void OnUpdate()
    {
        if (holdingRange && target)
        {
            fireTimer += Time.deltaTime;
            if (fireTimer >= fireCooldown)
            {
                fireTimer = 0f;
                Shoot(moveDirection);
            }
        }
    }

    private void Shoot(Vector2 direction)
    {

        if (projectilePrefab == null) return;
        Debug.Log("Hello");

        GameObject proj = Instantiate(projectilePrefab, transform.position, transform.rotation);
        Rigidbody2D projRb = proj.GetComponent<Rigidbody2D>();
        if (projRb != null)
        {
            projRb.linearVelocity = direction * projectileSpeed;
        }
    }
}
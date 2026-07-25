using UnityEngine;

public class MeleeEnemy : Enemy
{
    protected override void Move()
    {
        if (target)
        {
            rb.linearVelocity = moveDirection * CurrentSpeed;
        }
    }
}
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public float cooldown = 0.3f;
    protected float cooldownTimer = 0f;

    protected virtual void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    public bool CanAttack => cooldownTimer <= 0f;

    // Each weapon type implements its own attack behavior
    public abstract void Attack(Transform firePoint);

    protected void StartCooldown()
    {
        cooldownTimer = cooldown;
    }
}
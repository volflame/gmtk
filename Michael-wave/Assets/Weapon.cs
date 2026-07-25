using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public float cooldown = 0.3f;
    private float cooldownReadyTime = 0f;

    // No longer need Update() for cooldown tracking at all
    public bool CanAttack => Time.time >= cooldownReadyTime;

    public abstract void Attack(Transform firePoint);

    protected void StartCooldown()
    {
        cooldownReadyTime = Time.time + cooldown;
    }
}
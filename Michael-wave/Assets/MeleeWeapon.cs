using UnityEngine;

public class MeleeWeapon : Weapon
{
    public GameObject meleeHitbox;
    public float attackDuration = 0.3f;
    private float attackTimer = 0f;
    private bool isAttacking = false;

    private void Update()
    {
        if (isAttacking)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackDuration)
            {
                attackTimer = 0f;
                isAttacking = false;
                meleeHitbox.SetActive(false);
            }
        }
    }

    public override void Attack(Transform firePoint)
    {
        if (!CanAttack || isAttacking) return;

        meleeHitbox.SetActive(true);
        isAttacking = true;
        StartCooldown();
    }
}
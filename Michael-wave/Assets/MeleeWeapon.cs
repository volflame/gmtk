using UnityEngine;

public class MeleeWeapon : Weapon
{
    public GameObject meleeHitbox;
    public float attackDuration = 0.3f;
    private float attackTimer = 0f;
    private bool isAttacking = false;

    protected override void Update()
    {
        base.Update();

        if (isAttacking)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackDuration)
            {
                attackTimer = 0f;
                isAttacking = false;
                meleeHitbox.SetActive(false);
                StartCooldown(); // <-- moved here: cooldown begins AFTER the swing ends
            }
        }
    }

    public override void Attack(Transform firePoint)
    {
        if (!CanAttack || isAttacking) return;

        meleeHitbox.SetActive(true);
        isAttacking = true;
        // no StartCooldown() here anymore
    }
}
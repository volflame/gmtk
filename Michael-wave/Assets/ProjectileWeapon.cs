using UnityEngine;

public class ProjectileWeapon : Weapon
{
    public GameObject bulletPrefab;
    public float fireForce = 20f;

    public override void Attack(Transform firePoint)
    {
        if (!CanAttack) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);

        StartCooldown();
    }
}